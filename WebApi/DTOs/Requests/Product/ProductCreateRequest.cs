using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Requests.Product
{
    public record ProductCreateRequest(
    [Required] string Name,
    [Range(0.01, 100000)] decimal Price,
    string Description,
    [Required] string Color,  
    [Required] string Size   
);

}
