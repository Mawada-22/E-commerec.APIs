using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public record AddressDto
    {
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
        [Required]
        public string Street { get; init; }
        [Required]
        public string City { get; init; }
        [Required]
        public string Country { get; init; }
    }
}
