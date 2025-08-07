using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(IUserProductRepo repo,IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CartProducts([FromQuery] int productid, [FromQuery] int quantity, CancellationToken token)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var data = await repo.AddToCart(int.Parse(id), productid,token, quantity);
        return Ok(data);
    }
    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] PaginationQuerDto paginationQuer, CancellationToken token)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var data = await repo.GetCartListAsync(paginationQuer, int.Parse(id),token);
        var response = mapper.Map<List<CartDto>>(data.Items);
        return Ok(new PaginatedOrderResponse<CartDto>{ PageNumber = paginationQuer.PageNumber,PageSize = paginationQuer.PageSize,Count = data.TotalCount,Orders = response});
    }
    [HttpPost("proceed-to-buy")]
    public async Task<IActionResult> ProceedToBuy([FromQuery] int addressid, CancellationToken token)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var order = await repo.BuyFromCart(int.Parse(id),addressid,token);
        return Ok();
    }

}
