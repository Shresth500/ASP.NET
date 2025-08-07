using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]

[Authorize]
public class BuyController(IUserProductRepo repo,IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> BuyProduct([FromQuery] BuyProductQueryDto buyProductQueryDto, [FromQuery] int addressid)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var data = await repo.BuyProduct(int.Parse(id), buyProductQueryDto.ProductId, buyProductQueryDto.Quantity,addressid);
        var order = mapper.Map<OrderDto>(data);
        return Ok(order);
    }
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] PaginationQuerDto paginationQuer) 
    {
        var id = int.Parse(User!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var data = await repo.GetOrderListAsync(paginationQuer, id);
        var orderedProduct = mapper.Map<List<OrderDto>>(data.Items);
        return Ok(new PaginatedOrderResponse<OrderDto>{PageSize = paginationQuer.PageSize,PageNumber = paginationQuer.PageNumber,Count = data.TotalCount,Orders = orderedProduct});
    }
}
