using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IProductRepo
{
    Task<Product> addProductAsync(ProductRequestDto product, int userId,CancellationToken token);
    Task<ProductImage> UploadProductImagesAsync(int ProductId, ProductImage Image,CancellationToken token);
    Task<Product> updateProductAsync(ProductRequestDto product, int userid, int Id,CancellationToken token);
    Task<PagedResult<Product>> getProductsAsync(PaginationQuerDto paginationQuer,CancellationToken token);
    Task<Product> getProductByIdAsync(int ProductId, CancellationToken token);
}
