using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.User
{
    public record UserLoginRequest(
    [Required] string Email,
    [Required] string Password
);
}
