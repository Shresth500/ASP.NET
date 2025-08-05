using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class ProductImage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public string ImageName { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? Image { get; set; }
    public required string FileExtension { get; set; } = string.Empty;
    public long FileSizeInBytes { get; set; }
    public required string FilePath { get; set; } = string.Empty ;
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
