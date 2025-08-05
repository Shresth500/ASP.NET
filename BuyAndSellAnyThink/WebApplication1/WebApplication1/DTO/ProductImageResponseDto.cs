using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.DTO;

public class ProductImageResponseDto
{
    public int Id { get; set; }
    public string ImageName { get; set; } = string.Empty;
    public required FormFile Image { get; set; }
    public string FilePath { get; set; } = string.Empty;
}
