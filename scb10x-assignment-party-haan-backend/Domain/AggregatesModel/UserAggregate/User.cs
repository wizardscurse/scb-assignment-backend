using System;
namespace scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate
{
    public class User : BaseAggrefate
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Tel { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
