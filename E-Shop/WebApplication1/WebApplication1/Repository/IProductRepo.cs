using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IProductRepo
{
    Task<Product> addProductAsync(ProductRequestDto product, int userId);
    Task<ProductImage> UploadProductImagesAsync(int ProductId, ProductImage Image);
    Task<Product> updateProductAsync(ProductRequestDto product, int userid, int Id);
    Task<PagedResult<Product>> getProductsAsync(PaginationQuerDto paginationQuer);
    Task<Product> getProductByIdAsync(int ProductId);
}
