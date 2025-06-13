using System.ComponentModel.DataAnnotations;
using Core.Enums;


namespace WebApi.DTOs.Requests.Order
{
    public record OrderStatusUpdateRequest(
    [Required] OrderStatus Status  
);
}
