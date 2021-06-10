using System.Collections.Generic;
using System.Threading.Tasks;
using scb10x_assignment_party_haan_backend.Domain.DTOs.PartyHaanAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Services
{
    public interface IPartyHaanService
    {
        Task<List<PartyHaansResponse>> GetParties(int userId);

        Task<PartyHaansResponse> GetPartHaanById(int userId, int id);

        Task<PartyHaansResponse> CreatePartyHaan(int userId, PartyHaanRequest request);

        Task<PartyHaanResponse> UpdatePartyHaan(int userId, int partyHaanId, PartyHaanRequest request);

        Task<PartyHaanResponse> DeletePartyHaan(int userId, int partyHaanId);

        Task<PartyHaanResponse> AddPartyHaanMember(int userId, int partyHaanId);

        Task<PartyHaanResponse> DeletePartyHaanMember(int userId, int partyHaanId);
    }
}
