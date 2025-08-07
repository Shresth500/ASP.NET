namespace WebApplication1.DTO;

public class CartDetailsDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public List<ProductImageResponseDto> productImageResponseDtos { get; set; } = [];
}
