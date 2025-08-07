using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductRepo repo,IMapper mapper) : ControllerBase
{
    [Authorize(Roles ="Manufacturer")]
    [HttpPost]
    public async Task<IActionResult> addProduct([FromForm] ProductFormRequestDto productRequest,CancellationToken token)
    {
        var id = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var products = ProductImageValidation.convertToProductReuestDto(productRequest);
        var productImage = products.ProductImages;
        foreach (var image in productImage)
        {
            var validity = ProductImageValidation.ValidateFileUpdate(image);
            if (validity == 0)
                return BadRequest("File not Supported");
            if (validity == 2)
                return BadRequest("Image greater than 10 MB");
        }
        products.UserId = int.Parse(id);
        var userProduct = await repo.addProductAsync(products,int.Parse(id),token);
        var userProductResponse = mapper.Map<ProductResponseDto>(userProduct);
        return Ok(userProductResponse);
    }

    [HttpGet]
    public async Task<IActionResult> getProduct([FromQuery] PaginationQuerDto query, CancellationToken token)
    {
        var data = await repo.getProductsAsync(query,token);
        var products = mapper.Map<List<ProductResponseDto>>(data.Items);
        var productData = new PaginatedProductResponseDto
        {
            Count = products.Count,
            Products = products,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
        };
        return Ok(productData);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> getProductById([FromRoute] int id, CancellationToken token)
    {
        try
        {
            var data = await repo.getProductByIdAsync(id,token);
            var productData = mapper.Map<ProductResponseDto>(data);
            return Ok(productData);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles ="Manufacturer")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> updateProduct([FromRoute] int id, [FromForm] ProductFormRequestDto product,CancellationToken token)
    {
        var products = ProductImageValidation.convertToProductReuestDto(product);
        var userid = User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var productResponse = await repo.updateProductAsync(products,int.Parse(userid), id,token);
        var data = mapper.Map<ProductResponseDto>(productResponse);
        return Ok(data);
    }
}
