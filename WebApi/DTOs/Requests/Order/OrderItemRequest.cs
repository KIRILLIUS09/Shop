using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.Order
{
    public record OrderItemRequest(
        [Required] int ProductId,
        [Required][Range(1, 100)] int Quantity
    );
}