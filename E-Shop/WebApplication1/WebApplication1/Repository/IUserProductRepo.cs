using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IUserProductRepo
{
    public Task<Order> BuyProduct(int Id,int productId,int Quantity,int addressid, CancellationToken token);
    public Task<List<Cart>> AddToCart(int Id,int productId, CancellationToken token, int quantity = 1);
    public Task<Order> BuyFromCart(int Id, int addressid, CancellationToken token);
    public Task<PagedResult<Order>> GetOrderListAsync(PaginationQuerDto paginationQuerDto, int id,CancellationToken token);
    public Task<Order> GetOrderByIdAsync(int id,int UserId, CancellationToken token);
    public Task RemoveFromCart(int Id,int productId, CancellationToken token, int quantity=1);
    public Task<Cart> UpdateCart(int Id,int productId, CancellationToken token, int quantity=1);
    public Task<PagedResult<Cart>> GetCartListAsync(PaginationQuerDto paginationQuerDto, int id, CancellationToken token);
    public Task<Cart> GetCartByIdAsync(int id,int UserId, CancellationToken token);
}
