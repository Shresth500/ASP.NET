using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class AdminRepo(ApplicationDbContext context) : IAdminRepo
{
    public async Task approveAll()
    {
        var data = await context.Product.ToListAsync();
        foreach (var item in data)
            item.isApproved = ApprovedStatus.Approved;

        await context.SaveChangesAsync();
    }

    public async Task ChangeListOfProductsStatus(List<int> productIds,ApprovedStatus status)
    {
        var data = await context.Product
                            .Where(x => productIds.Contains(x.Id))
                           .ToListAsync();

        foreach (var item in data)
            item.isApproved = status;
        await context.SaveChangesAsync();
    }

    public async Task<bool> ChangeRole(int id,string role)
    {
        var user = await context.User.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return false;
        user!.Role = role;
        return true;
    }

    public Task ChangetheProductStatusByIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Product>> getNonActiveProductByAsync(PaginationQuerDto paginationQuer)
    {
        var data = await context.Product
            .Where(x => x.isApproved == paginationQuer.IsApproved)
            .Include(y => y.ProductImages)
            .Skip((paginationQuer.PageNumber - 1) * paginationQuer.PageSize)
            .Take(paginationQuer.PageSize).ToListAsync();

        return new PagedResult<Product>{TotalCount = data.Count ,Items = data};
    }

}
