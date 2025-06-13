namespace WebApi.DTOs.Requests.Product
{
    public record ProductUpdateRequest(
    string? Name,
    decimal? Price,
    string? Description,
    string? Color,
    string? Size
);
}
