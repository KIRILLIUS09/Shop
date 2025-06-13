namespace WebApi.DTOs.Responses.Cart
{
    public record CartItemResponse(
    int Id,
    int ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity,
    decimal TotalPrice  
);
}
