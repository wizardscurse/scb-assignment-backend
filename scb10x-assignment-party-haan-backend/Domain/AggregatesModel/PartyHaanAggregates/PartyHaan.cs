using System;
using System.Collections.Generic;

namespace scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate
{
    public class PartyHaan : BaseAggrefate
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public int? Limit { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public List<PartyHaanMember> PartyHaanMembers { get; set; }
    }
}
