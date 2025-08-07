using Microsoft.EntityFrameworkCore;
using WebApplication1.Common;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class ProductRepo(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IProductRepo
{

    public async Task<Product> addProductAsync(ProductRequestDto product,int userId, CancellationToken token)
    {
        var productListImages = new List<ProductImage>();
        var productData = new Product
        {
            Name = product.Name,
            Quantity = product.Quantity,
            Description = product.Description,
            Price = product.Price,
            Benefits = product.Benefits,
            ProductImages = productListImages,
            UserId = userId
        };
        await context.Product.AddAsync(productData,token);
        await context.SaveChangesAsync(token);
        var productImages = product.ProductImages.ToList();
        foreach (var image in productImages)
        {
            var folderPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", productData.Id.ToString());
            Directory.CreateDirectory(folderPath); // ensure folder exists

            var extension = Path.GetExtension(image.Image.FileName);
            var fileName = Path.GetFileNameWithoutExtension(image.Image.FileName);
            var fullFileName = fileName + extension;
            var localfilepath = Path.Combine(folderPath, fullFileName);

            using var stream = new FileStream(localfilepath, FileMode.Create);
            await image.Image.CopyToAsync(stream, token);

            var urlFilepath = $"{httpContextAccessor.HttpContext!.Request.Scheme}://" +
                              $"{httpContextAccessor.HttpContext!.Request.Host}" +
                              $"{httpContextAccessor.HttpContext.Request.PathBase}/Images/{productData.Id}/{fullFileName}";

            var imageProduct = new ProductImage
            {
                ImageName = fileName,
                FileExtension = extension,
                FilePath = urlFilepath,
                FileSizeInBytes = image.Image.Length,
                ProductId = productData.Id,
                Image = image.Image
            };
            productListImages.Add(imageProduct);
        }

        await context.ProductImages.AddRangeAsync(productListImages, token);
        await context.SaveChangesAsync(token);
        return productData;
    }

    public async Task<Product> getProductByIdAsync(int ProductId, CancellationToken token)
    {
        var data = await context.Product
            .Include(a => a.ProductImages)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == ProductId, token);
        if (data == null)
            throw new Exception("Data not found");
        return data;
    }

    public async Task<PagedResult<Product>> getProductsAsync(PaginationQuerDto paginationQuer, CancellationToken token)
    {
        var productData = await context.Product
            .Where(a => a.isApproved==paginationQuer.IsApproved)
            .AsNoTracking()
            .Skip((paginationQuer.PageNumber - 1) * paginationQuer.PageSize)
            .Take(paginationQuer.PageSize)
            .Include(x => x.ProductImages).ToListAsync(token);
        var totalCount = productData.Count;
        return new PagedResult<Product> { Items = productData,TotalCount = totalCount};
    }

    public async Task<Product> updateProductAsync(ProductRequestDto product,int userid, int Id,CancellationToken token)
    {
        var data = await context.Product.Include(a => a.ProductImages).FirstOrDefaultAsync(x => x.Id == Id);
        if (data == null)
            throw new Exception("Product Not Found");
        if (data.UserId != userid)
            throw new Exception("Unauthorized to update the product");
        data.Name = product.Name;
        data.Description = product.Description;
        data.Price = product.Price;
        data.Quantity = product.Quantity;
        data.Benefits = product.Benefits;
        context.ProductImages.RemoveRange(data.ProductImages!);
        ProductImageValidation.DeleteFolder(Id);
        var productImages = product.ProductImages.ToList();
        var newProductImage = new List<ProductImage>();
        foreach (var image in productImages) 
        {
            var folderPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", Id.ToString());
            Directory.CreateDirectory(folderPath); // ensure folder exists

            var extension = Path.GetExtension(image.Image.FileName);
            var fileName = Path.GetFileNameWithoutExtension(image.Image.FileName);
            var fullFileName = fileName + extension;
            var localfilepath = Path.Combine(folderPath, fullFileName);

            using var stream = new FileStream(localfilepath, FileMode.Create);
            await image.Image.CopyToAsync(stream,token);

            var urlFilepath = $"{httpContextAccessor.HttpContext!.Request.Scheme}://" +
                              $"{httpContextAccessor.HttpContext!.Request.Host}" +
                              $"{httpContextAccessor.HttpContext.Request.PathBase}/Images/{Id}/{fullFileName}";

            var imageProduct = new ProductImage
            {
                ImageName = fileName,
                FileExtension = extension,
                FilePath = urlFilepath,
                FileSizeInBytes = image.Image.Length,
                ProductId = Id,
                Image = image.Image
            };
            newProductImage.Add(imageProduct);
        }
        data.ProductImages = newProductImage;
        await context.SaveChangesAsync(token);
        return data;
    }

    public async Task<ProductImage> UploadProductImagesAsync(int ProductId, ProductImage Image, CancellationToken token)
    {
        var folderPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", ProductId.ToString());
        Directory.CreateDirectory(folderPath); // ensure folder exists

        var fileName = Image.ImageName + Image.FileExtension;
        var localfilepath = Path.Combine(folderPath, fileName);

        using var stream = new FileStream(localfilepath, FileMode.Create);
        await Image!.Image!.CopyToAsync(stream, token);
        var urlFilepath = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{ProductId}/{Image.ImageName}{Image.FileExtension}";
        Image.FilePath = urlFilepath;
        await context.ProductImages.AddAsync(Image);
        await context.SaveChangesAsync(token);
        return Image;
    }

}
