using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using scb10x_assignment_party_haan_backend.Domain.DTOs;
using scb10x_assignment_party_haan_backend.Domain.Exceptions;
using scb10x_assignment_party_haan_backend.Infrastructure.DataContexts;
using scb10x_assignment_party_haan_backend.Infrastructure.Repositories;
using scb10x_assignment_party_haan_backend.Infrastructure.Services;

namespace scb10x_assignment_party_haan_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            IdentityModelEventSource.ShowPII = true;
            var token = Configuration.GetSection("tokenSetting").Get<TokenSetting>();
            var secret = Encoding.ASCII.GetBytes(token.Secret);

            services.Configure<TokenSetting>(Configuration.GetSection("tokenSetting"));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services
                .AddDbContext<PartyHaanContext>(options =>
                    options.UseInMemoryDatabase("EntityFramework:DatabaseName"))
                .AddTransient<IPartyHaanService, PartyHaanService>()
                .AddTransient<IUserService, UserService>()
                .AddSingleton<IPartyHaanRepository, PartyHaanRepository>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddCors()
                .AddControllers(options =>
                    options.Filters.Add(new HttpResponseExceptionFilter()));

            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseCors(e => e.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
