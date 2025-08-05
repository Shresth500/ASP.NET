using WebApplication1.DTO;

namespace WebApplication1.Common;

public static class ProductImageValidation
{
    public static int ValidateFileUpdate(ProductImageRequestDto productImageRequest)
    {
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(Path.GetExtension(productImageRequest.Image.FileName)))
            return 0;
        if (productImageRequest.Image.Length > 10 * 1024 * 1024)
            return 2;
        return 1;
    }

    public static ProductRequestDto convertToProductReuestDto(ProductFormRequestDto productFormRequest)
    {
        var products = new ProductRequestDto
        {
            Name = productFormRequest.Name,
            Quantity = productFormRequest.Quantity,
            Price = productFormRequest.Price,
            Description = productFormRequest.Description,
            Benefits = productFormRequest.Benefits,
            ProductImages = new List<ProductImageRequestDto>()
        };
        foreach (var item in productFormRequest.ProductImages)
        {
            products.ProductImages.Add(new ProductImageRequestDto
            {
                Image = item,
                ImageName = item.FileName,
            });
        }

        return products;
    }

    public static void DeleteFolder(int productId)
    {
        var folderName = Path.Combine("..", "Images", productId.ToString());
        var folderPath = Path.GetFullPath(folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, recursive: true);
            Console.WriteLine("Folder deleted successfully.");
        }
        else
        {
            Console.WriteLine("Folder does not exist: " + folderPath);
        }
    }
}
