namespace WebApplication1.DTO;

public class ProductFormRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public List<IFormFile> ProductImages { get; set; } = [];
}
