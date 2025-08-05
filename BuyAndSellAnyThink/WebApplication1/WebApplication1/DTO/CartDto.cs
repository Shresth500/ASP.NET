namespace WebApplication1.DTO;

public class CartDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public ProductResponseDto Product { get; set; }
}
