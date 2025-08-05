namespace WebApplication1.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<OrderDetailDto>? OrderItems { get; set; }
}
