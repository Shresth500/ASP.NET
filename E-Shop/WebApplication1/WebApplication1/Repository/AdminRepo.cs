using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class AdminRepo(ApplicationDbContext context) : IAdminRepo
{
    public async Task approveAll(CancellationToken token)
    {
        var data = await context.Product.ToListAsync(token);
        foreach (var item in data)
            item.isApproved = ApprovedStatus.Approved;

        await context.SaveChangesAsync(token);
    }

    public async Task ChangeListOfProductsStatus(List<int> productIds,ApprovedStatus status, CancellationToken token)
    {
        var data = await context.Product
                            .Where(x => productIds.Contains(x.Id))
                           .ToListAsync(token);

        foreach (var item in data)
            item.isApproved = status;
        await context.SaveChangesAsync(token);
    }

    public async Task<bool> ChangeRole(int id,string role, CancellationToken token)
    {
        var user = await context.User.FirstOrDefaultAsync(x => x.Id == id,token);
        if (user == null)
            return false;
        user!.Role = role;
        return true;
    }

    public Task ChangetheProductStatusByIdAsync(int productId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Product>> getNonActiveProductByAsync(PaginationQuerDto paginationQuer,CancellationToken token)
    {
        var data = await context.Product
            .Where(x => x.isApproved == paginationQuer.IsApproved)
            .Include(y => y.ProductImages)
            .AsNoTracking()
            .Skip((paginationQuer.PageNumber - 1) * paginationQuer.PageSize)
            .Take(paginationQuer.PageSize).ToListAsync(token);

        return new PagedResult<Product>{TotalCount = data.Count ,Items = data};
    }

}
