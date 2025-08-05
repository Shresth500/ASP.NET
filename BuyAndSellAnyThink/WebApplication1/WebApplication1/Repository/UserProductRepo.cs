using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class UserProductRepo(ApplicationDbContext context) : IUserProductRepo
{
    public async Task<List<Cart>> AddToCart(int userId, int productId, int quantity = 1)
    {
        var data = await context.Cart.Where(x => x.UserId == userId).ToListAsync();

        var productDetails = await context.Product.FirstOrDefaultAsync(x => x.Id == productId);
        if (productDetails == null || productDetails.Quantity < quantity)
            throw new Exception($"Quantity of {productDetails?.Name ?? "product"} is less than the required quantity");

        Cart? cart = data.FirstOrDefault(x => x.ProductId == productId);

        if (cart != null)
        {
            cart.Quantity += quantity;
            cart.Price = cart.Quantity * productDetails.Price; 
        }
        else
        {
            cart = new Cart
            {
                ProductId = productId,
                UserId = userId,
                Quantity = quantity,
                Price = quantity * productDetails.Price
            };
            await context.Cart.AddAsync(cart);
        }

        await context.SaveChangesAsync();
        return await context.Cart.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<Order> BuyFromCart(int userId)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var cartItems = await context.Cart
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if (!cartItems.Any())
                throw new Exception("Cart is empty.");
            var productIds = cartItems.Select(c => c.ProductId).ToList();
            var products = await context.Product
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var item in cartItems)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    throw new Exception($"Product ID {item.ProductId} not found.");

                if (item.Quantity > product.Quantity)
                    throw new Exception($"Not enough stock for product {product.Name}.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderItems = new List<OrderItems>()
            };

            foreach (var item in cartItems)
            {
                var product = products[item.ProductId];
                order.OrderItems.Add(new OrderItems
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });

                product.Quantity -= item.Quantity;
            }

            await context.Order.AddAsync(order);
            context.Cart.RemoveRange(cartItems);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<Order> BuyProduct(int Id, int productId, int Quantity)
    {
        var productdetails = await context.Product.Where(x => x.Id == productId).FirstOrDefaultAsync();
        if (Quantity > productdetails!.Quantity)
            throw new Exception($"Quantity of {productdetails.Name} is less than the required quantity");
        var order = new Order
        {
            UserId = Id,
            OrderItems = new List<OrderItems>
                {
                    new OrderItems
                    {
                        ProductId = productId,
                        Quantity = Quantity,
                        Price = Quantity * productdetails.Price
                    }
                }
        };
        await context.Order.AddAsync(order);
        productdetails.Quantity -= Quantity;
        await context.SaveChangesAsync();

        return order;
    }

    public Task<Cart> GetCartByIdAsync(int id, int UserId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Cart>> GetCartListAsync(PaginationQuerDto paginationQuerDto, int id)
    {
        var query = context.Cart!
            .Where(x => x.UserId == id)
            .Include(b => b.Product)
            .ThenInclude(c => c.ProductImages);

        var totalCount = await query.CountAsync();
        var item = await query.Skip((paginationQuerDto.PageNumber - 1) * paginationQuerDto.PageSize).Take(paginationQuerDto.PageSize).ToListAsync();
        return new PagedResult<Cart> { TotalCount = totalCount, Items = item };
    }

    public Task<Order> GetOrderByIdAsync(int id, int UserId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Order>> GetOrderListAsync(PaginationQuerDto paginationQuerDto, int userId)
    {
        var query = context.Order!
            .Where(a => a.UserId == userId)!
            .Include(b => b.OrderItems)!
                .ThenInclude(c => c.Product)
                    .ThenInclude(d => d.ProductImages);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((paginationQuerDto.PageNumber - 1) * paginationQuerDto.PageSize)
            .Take(paginationQuerDto.PageSize).ToListAsync();

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
    public async Task RemoveFromCart(int userId, int productId, int quantity = 1)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be at least 1.");

        var cart = await context.Cart
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);

        if (cart == null)
            throw new Exception("Cart item not found.");

        if (cart.Quantity <= quantity)
            context.Cart.Remove(cart);
        else
        {
            var unitPrice = cart.Price / cart.Quantity;
            cart.Quantity -= quantity;
            cart.Price -= unitPrice * quantity;
        }
        await context.SaveChangesAsync();
    }
    public Task<Cart> UpdateCart(int Id, int productId, int quantity)
    {
        throw new NotImplementedException();
    }
}
