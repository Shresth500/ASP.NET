using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.DTO;

public class ProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public List<ProductImageRequestDto> ProductImages { get; set; } = [];
}
