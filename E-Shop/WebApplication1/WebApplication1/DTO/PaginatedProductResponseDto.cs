namespace WebApplication1.DTO;

public class PaginatedProductResponseDto
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<ProductResponseDto>? Products { get; set; }
}
