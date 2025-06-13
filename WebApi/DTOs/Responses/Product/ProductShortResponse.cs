namespace WebApi.DTOs.Responses.Product
{
    public record ProductShortResponse(
    int Id,
    string Name,
    decimal Price,
    string Color
);
}
