namespace WebApplication1.DTO;

public class PaginatedOrderResponse<T>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int Count { get; set; }
    public List<T>? Orders { get; set; }
}

