namespace WebApplication1.Models;

public class Address
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;  
    public string Country { get; set; } = string.Empty;
    public string Street {  get; set; } = string.Empty ;
    public List<Order> Orders { get; set; } = new List<Order>();
}
