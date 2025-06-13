using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.User
{
    public record UserRegisterRequest(
    [Required, EmailAddress] string Email,
    [Required] string Username,
    [MinLength(8)] string Password,
    [Required] string FirstName,
    [Required] string LastName,
    [Required, Phone] string PhoneNumber,
    string? MiddleName = null
);
}
