using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Infrastructure.DataContexts;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IServiceScope _scope;
        private readonly IConfiguration _configuration;
        private readonly PartyHaanContext _partyHaanContext;

        public UserRepository(IConfiguration configuration, IServiceProvider services)
        {
            _scope = services.CreateScope();
            _configuration = configuration;
            _partyHaanContext = _scope.ServiceProvider.GetRequiredService<PartyHaanContext>();
        }

        public async Task<List<User>> GetUsers()
        {
            var result = await _partyHaanContext.Users
                                .OrderByDescending(m => m.Email)
                                .ToListAsync();

            return result;
        }


        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var user = await _partyHaanContext.Users
                                .FirstOrDefaultAsync(m => m.Id.Equals(id));

            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Tel = user.Tel,
            };
        }

        public async Task<UserResponseDTO> SignUp(SignUpRequest request)
        {
            try
            {
                var user = await _partyHaanContext.Users
                                .FirstOrDefaultAsync(m => m.Email.Equals(request.Email));
                if (user != null)
                    throw new Exception("มีอีเมลนี้ในระบบอยู่แล้ว");

                var userData = new User
                {
                    Email = request.Email,
                    Password = Base64Encode(request.Password),
                    Name = request.Name,
                    Tel = request.Tel,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                _partyHaanContext.Users.Add(userData);

                await _partyHaanContext.SaveChangesAsync();

                return new UserResponseDTO
                {
                    Id = userData.Id,
                    Email = userData.Email,
                    Name = userData.Name,
                    Tel = userData.Tel,
                    Token = GenerateToken(userData.Id.ToString())
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<UserResponseDTO> SignIn(SignInRequestDTO request)
        {
            try
            {
                var user = await _partyHaanContext.Users
                                .FirstOrDefaultAsync(m => m.Email.Equals(request.Email) && Base64Decode(m.Password).Equals(request.Password));
                if (user == null)
                    throw new Exception("อีเมลหรือรหัสผ่านไม่ถูกต้อง");

                return new UserResponseDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Tel = user.Tel,
                    Token = GenerateToken(user.Id.ToString()),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string GenerateToken(string id)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenSetting = _configuration.GetSection("tokenSetting").Get<TokenSetting>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSetting.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToInt32(tokenSetting.AccessExpiration));
            var token = new JwtSecurityToken(
                issuer: tokenSetting.Issuer,
                audience: tokenSetting.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );


            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
