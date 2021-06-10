using System.Collections.Generic;
using System.Threading.Tasks;
using scb10x_assignment_party_haan_backend.Domain.DTOs.PartyHaanAggregate;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Infrastructure.Repositories;

namespace scb10x_assignment_party_haan_backend.Infrastructure.Services
{
    public class PartyHaanService : IPartyHaanService
    {
        private readonly IPartyHaanRepository _partyHaanRepository;
        private readonly IUserRepository _userRepository;

        public PartyHaanService(IPartyHaanRepository partyHaanRepository, IUserRepository userRepository)
        {
            _partyHaanRepository = partyHaanRepository;
            _userRepository = userRepository;
        }

        public async Task<List<PartyHaansResponse>> GetParties(int userId)
        {
            var result = await _partyHaanRepository.GetParties(userId);

            return result;
        }

        public async Task<PartyHaansResponse> GetPartHaanById(int userId, int id)
        {
            var result = await _partyHaanRepository.GetPartHaanById(userId, id);

            return result;
        }

        public async Task<PartyHaansResponse> CreatePartyHaan(int userId, PartyHaanRequest request)
        {
            return await _partyHaanRepository.CreatePartyHaan(userId, request);
        }

        public async Task<PartyHaanResponse> UpdatePartyHaan(int userId, int partyHaanId, PartyHaanRequest request)
        {
            return await _partyHaanRepository.UpdatePartyHaan(userId, partyHaanId, request);
        }

        public async Task<PartyHaanResponse> DeletePartyHaan(int userId, int partyHaanId)
        {
            return await _partyHaanRepository.DeletePartyHaan(userId, partyHaanId);
        }

        public async Task<PartyHaanResponse> AddPartyHaanMember(int userId, int partyHaanId)
        {
            return await _partyHaanRepository.AddPartyHaanMember(userId, partyHaanId);
        }

        public async Task<PartyHaanResponse> DeletePartyHaanMember(int userId, int partyHaanId)
        {
            return await _partyHaanRepository.DeletePartyHaanMember(userId, partyHaanId);
        }

    }
}
