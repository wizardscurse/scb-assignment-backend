using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate
{
    public class PartyHaanMember : BaseAggrefate
    {
        [ForeignKey("PartHaanRefId")]
        public PartyHaan PartyHaan { get; set; }

        public int PartHaanRefId { get; set; }

        [ForeignKey("UserRefId")]
        public User User { get; set; }
        
        public int UserRefId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }
    }
}
