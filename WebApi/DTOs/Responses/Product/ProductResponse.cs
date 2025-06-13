namespace WebApi.DTOs.Responses.Product
{
    public record ProductResponse(
     int Id,
     string Name,
     decimal Price,
     string Description,
     string Color,
     string Size
 );
}
