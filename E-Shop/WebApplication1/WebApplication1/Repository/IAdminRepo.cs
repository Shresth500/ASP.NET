using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAdminRepo
{
    public Task<PagedResult<Product>> getNonActiveProductByAsync(PaginationQuerDto paginationQuer, CancellationToken token);
    public Task ChangetheProductStatusByIdAsync(int productId, CancellationToken token);
    public Task ChangeListOfProductsStatus(List<int> productIds,ApprovedStatus status, CancellationToken token);
    public Task approveAll(CancellationToken token);
    public Task<bool> ChangeRole(int id,string role, CancellationToken token);
}
