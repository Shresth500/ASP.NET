using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles ="Admin")]
public class AdminWorkController(IAdminRepo repo, IMapper mapper) : ControllerBase
{
    [HttpGet("NonApprovedProducts")]
    public async Task<IActionResult> getNonApprovedProducts([FromQuery]PaginationQuerDto paginationQuerDto, CancellationToken token)
    {
        var data = await repo.getNonActiveProductByAsync(paginationQuerDto,token);
        var products = mapper.Map<List<ProductResponseDto>>(data.Items);
        return Ok(new PaginatedProductResponseDto { Count = data.TotalCount,PageNumber = paginationQuerDto.PageNumber,PageSize = paginationQuerDto.PageSize,Products = products});
    }
    [HttpPost("Approve")]
    public async Task<IActionResult> ChangeTheProductStatusById([FromQuery] List<int> productIds,[FromQuery]ApprovedStatus status, CancellationToken token)
    {
        await repo.ChangeListOfProductsStatus(productIds,status,token);
        return Ok();
    }
    [HttpPost("ApproveAll")]
    public async Task<IActionResult> ApproveAll(CancellationToken token)
    {
        await repo.approveAll(token);
        return Ok();
    }
    [HttpPost("ChangeUserRole")]
    public async Task<IActionResult> ChangeUserRole([FromQuery]int id, [FromQuery] string role, CancellationToken token)
    {
        var result = await repo.ChangeRole(id,role,token);
        if (!result)
            return BadRequest("User not found");
        return Ok("Role changed");
    }
}
