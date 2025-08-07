using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public enum ApprovedStatus
{
    Rejected,
    Approved,
    Pending
}
public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public ApprovedStatus isApproved { get; set; } = 0;
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<ProductImage>? ProductImages { get; set; }
    public List<Cart>? Cart {  get; set; }
    public List<OrderItems>? OrderItem {  get; set; }
}
