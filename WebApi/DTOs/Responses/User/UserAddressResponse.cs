namespace WebApi.DTOs.Responses.User
{
    public record UserAddressResponse(
    int Id,
    string FullAddress,
    string Region,
    string City
);
}
