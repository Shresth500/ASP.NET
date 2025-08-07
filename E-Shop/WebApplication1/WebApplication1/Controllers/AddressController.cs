using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AddressController(IAddressRepo repo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAddressAsync(PaginationQuerDto pagination,CancellationToken token)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var data = await repo.findAddressAsync(pagination,int.Parse(id),token);
        var result = mapper.Map<List<AddressDto>>(data.Items);
        return Ok(new { Count = data.TotalCount, PageNumber = pagination.PageNumber, PageSize = pagination.PageSize, Address = result });
    }
    [HttpGet("{id:int}")]
    public  async Task<IActionResult> GetAddressByIdAsync([FromRoute] int id,CancellationToken token)
    {
        var userId = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var data = await repo.findAddressByIdAsync(id, int.Parse(userId),token);
        var result = mapper.Map<AddressDto>(data);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> AddAddress(AddressDto address, CancellationToken token)
    {
        var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null) return Unauthorized();
        var data = await repo.AddAddressAsync(address, int.Parse(id),token);
        var addedAddress = mapper.Map<AddressDto>(data);
        return Ok(addedAddress);
    }
    [HttpPut("{AddressId:int}")]
    public async Task<IActionResult> UpdateAddress([FromBody] AddressDto address, [FromRoute] int AddressId,CancellationToken token)
    {
        var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null) return Unauthorized();
        await repo.UpdateAddressAsync(address, AddressId, int.Parse(id),token);
        return Ok();
    }

    [HttpDelete("delete{AddressId:int}")]
    public async Task<IActionResult> DeleteAddress([FromRoute] int AddressId, CancellationToken token)
    {
        var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null) return Unauthorized();
        await repo.DeleteAddressAsync(AddressId, int.Parse(id),token);
        return Ok();
    }
}
