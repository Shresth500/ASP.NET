using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class AddressRepo(ApplicationDbContext context) : IAddressRepo
{
    public async Task<Address> AddAddressAsync(AddressDto address, int id)
    {
        var data = new Address
        {
            UserId = id,
            State = address.State,
            Street = address.Street,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
        };
        await context.Address.AddAsync(data);
        await context.SaveChangesAsync();
        return data;
    }
    public async Task DeleteAddressAsync(int AddressId, int userid)
    {
        var data = await context.Address.FindAsync(AddressId);
        if (data is null) throw new Exception("Address not available");
        if (data.UserId != userid) throw new Exception("UnAuthorized");
        context.Remove(data!);
        await context.SaveChangesAsync();
    }
    public async Task<PagedResult<Address>> findAddressAsync(PaginationQuerDto paginationQuerDto, int userId)
    {
        var query = context.Address!
            .Where(x => x.UserId == userId);

        var totalCount = await query.CountAsync();
        var addressData = await query.Skip((paginationQuerDto.PageNumber - 1) * paginationQuerDto.PageSize).Take(paginationQuerDto.PageSize).ToListAsync();
        return new PagedResult<Address> { TotalCount = totalCount, Items = addressData };
    }

    public async Task<Address> findAddressByIdAsync(int id, int userId)
    {
        var data = await context.Address.FindAsync(id, userId);
        if (data is null) throw new Exception("Address not found");
        return data;
    }
    public async Task UpdateAddressAsync(AddressDto address, int AddressId, int userid)
    {
        var data = await context.Address.FindAsync(AddressId);
        if (data is null) throw new Exception("Address not available");
        if (data.UserId != userid) throw new Exception("UnAuthorized");
        data!.Street = address.Street;
        data!.City = address.City;
        data!.State = address.State;
        data!.PostalCode = address.PostalCode;
        data!.Country = address.Country;
        await context.SaveChangesAsync();
    }
}
