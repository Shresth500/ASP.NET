using WebApplication1.Models;

namespace WebApplication1.DTO;

public class OrderDetailDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public required ProductResponseDto ProductDetails { get; set; }
}
