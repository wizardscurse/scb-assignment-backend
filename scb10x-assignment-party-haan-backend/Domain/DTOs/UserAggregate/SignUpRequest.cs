using System.ComponentModel.DataAnnotations;

namespace scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate
{
    public class SignUpRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string Tel { get; set; }
    }
}
