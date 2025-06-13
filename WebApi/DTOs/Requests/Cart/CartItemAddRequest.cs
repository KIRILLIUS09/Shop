using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.Cart
{
    public record CartItemAddRequest(
        [Required(ErrorMessage = "ProductId обязателен")]
        int ProductId,

        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        int Quantity = 1 
    );
}
