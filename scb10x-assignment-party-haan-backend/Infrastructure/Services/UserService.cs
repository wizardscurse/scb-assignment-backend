using System.Collections.Generic;
using System.Threading.Tasks;
using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Infrastructure.Repositories;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUsers()
        {
            var result = await _userRepository.GetUsers();

            return result;
        }

        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var result = await _userRepository.GetUserById(id);

            return result;
        }

        public async Task<UserResponseDTO> SignUp(SignUpRequest request)
        {
            return await _userRepository.SignUp(request);
        }

        public async Task<UserResponseDTO> SignIn(SignInRequestDTO request)
        {
            return await _userRepository.SignIn(request);
        }
    }
}
