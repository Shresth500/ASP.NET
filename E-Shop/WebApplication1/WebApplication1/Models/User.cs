using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email {  get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty.ToString();
    public List<Address>? AddressList { get; set; }
    public List<Product> Products { get; set; } = [];
    public List<Cart>? Cart { get; set; } = [];
    public List<Order>? Order { get; set; } = [];
    public List<Notification>? Notifications { get; set; } = [];
}
