using Core.Enums;
using WebApi.DTOs.Responses.User;

namespace WebApi.DTOs.Responses.Order
{
    public record OrderResponse(
    int Id,
    DateTime CreationDate,
    OrderStatus Status,
    decimal TotalAmount,
    UserAddressResponse ShippingAddress,
    List<OrderItemResponse> Items,
    string? Comment = null 
);
}
