namespace WebApi.DTOs.Requests.Product
{
    public record ProductFilterRequest(
     string? Color,
     string? Size,
     decimal? MinPrice,
     decimal? MaxPrice
 );
}
