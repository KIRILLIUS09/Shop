using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace WebApi.DTOs.Requests.Order
{
    public record OrderCreateRequest(
    [Required] int AddressId,
    [Required] int UserId,
    [Required] List<OrderItemRequest> Items,
    string? Comment = null
);
}