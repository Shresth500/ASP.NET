

namespace WebApplication1.DTO;

public class ProductImageRequestDto
{
    public string ImageName { get; set; } = string.Empty;
    public required IFormFile Image { get; set; }
 
}
