namespace WebApi.DTOs.Responses.Cart
{
    public record CartResponse(
        List<CartItemResponse> Items,
        decimal Total
    );
}
