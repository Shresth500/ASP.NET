using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAdminRepo
{
    public Task<PagedResult<Product>> getNonActiveProductByAsync(PaginationQuerDto paginationQuer);
    public Task ChangetheProductStatusByIdAsync(int productId);
    public Task ChangeListOfProductsStatus(List<int> productIds,ApprovedStatus status);
    public Task approveAll();
    public Task<bool> ChangeRole(int id,string role);
}
