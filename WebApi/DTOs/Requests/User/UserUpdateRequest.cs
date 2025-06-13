using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.User
{
    public record UserUpdateRequest(
        string? FirstName,
        string? LastName,
        [Phone] string? PhoneNumber
    );
}
