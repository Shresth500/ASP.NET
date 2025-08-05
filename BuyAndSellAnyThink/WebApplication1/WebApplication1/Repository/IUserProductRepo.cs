using Microsoft.AspNetCore.Routing.Constraints;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IUserProductRepo
{
    public Task<Order> BuyProduct(int Id,int productId,int Quantity);
    public Task<List<Cart>> AddToCart(int Id,int productId,int quantity = 1);
    public Task<Order> BuyFromCart(int Id);
    public Task<PagedResult<Order>> GetOrderListAsync(PaginationQuerDto paginationQuerDto, int id);
    public Task<Order> GetOrderByIdAsync(int id,int UserId);
    public Task RemoveFromCart(int Id,int productId,int quantity=1);
    public Task<Cart> UpdateCart(int Id,int productId,int quantity=1);
    public Task<PagedResult<Cart>> GetCartListAsync(PaginationQuerDto paginationQuerDto, int id);
    public Task<Cart> GetCartByIdAsync(int id,int UserId);
}
