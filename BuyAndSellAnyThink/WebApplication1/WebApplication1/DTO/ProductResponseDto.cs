using WebApplication1.Models;

namespace WebApplication1.DTO;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public ApprovedStatus isApproved { get; set; }
    public int UserId { get; set; }
    public List<ProductImageResponseDto> ProductImages { get; set; } = [];
}
