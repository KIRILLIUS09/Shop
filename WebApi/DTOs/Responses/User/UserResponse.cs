namespace WebApi.DTOs.Responses.User
{
    public record UserResponse(
    int Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string PhoneNumber
);
}
