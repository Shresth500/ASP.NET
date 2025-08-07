using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Notification
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User? User { get; set; }
}
