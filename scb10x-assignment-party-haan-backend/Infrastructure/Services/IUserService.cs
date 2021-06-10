using System.Collections.Generic;
using System.Threading.Tasks;
using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();

        Task<UserResponseDTO> SignUp(SignUpRequest request);

        Task<UserResponseDTO> SignIn(SignInRequestDTO request);

        Task<UserResponseDTO> GetUserById(int id);
    }
}
