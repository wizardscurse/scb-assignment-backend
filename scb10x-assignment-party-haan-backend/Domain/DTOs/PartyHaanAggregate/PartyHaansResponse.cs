using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;

namespace scb10x_assignment_party_haan_backend.Domain.DTOs.PartyHaanAggregate
{
    public class PartyHaansResponse: PartyHaan
    {
        public int? CurrentMember { get; set; }

        public bool Joined { get; set; }
    }
}
