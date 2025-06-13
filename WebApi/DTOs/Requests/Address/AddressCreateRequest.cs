using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.Address
{
    public record AddressCreateRequest(
        [Required] string Region,
        [Required] string City,
        [Required] string Street,
        [Required] string Building,
        string? Apartment,
        [Required] int UserId
    );
}