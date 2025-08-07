namespace WebApplication1.Models;

public class Cart
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int Quantity { get; set; }
    public int Price { get; set; }
}
