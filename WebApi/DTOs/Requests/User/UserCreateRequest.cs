using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.User
{
    public record UserCreateRequest(
        [Required][EmailAddress] string Email,
        [Required][MinLength(3)] string Username,
        [Required][MinLength(8)] string Password,
        [Required] string FirstName,
        [Required] string LastName,
        [Phone] string PhoneNumber
    );
}
