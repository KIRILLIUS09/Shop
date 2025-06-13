namespace WebApi.DTOs.Responses.Order
{
    public record OrderItemResponse(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal PricePerUnit
);
}
