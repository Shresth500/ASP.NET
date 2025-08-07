using WebApplication1.Models;

namespace WebApplication1.DTO;

public class PaginationQuerDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public ApprovedStatus IsApproved { get; set; } = ApprovedStatus.Rejected;
}
