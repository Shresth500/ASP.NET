using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAddressRepo
{
    Task<PagedResult<Address>> findAddressAsync(PaginationQuerDto paginationQuerDto, int userId);
    Task<Address> findAddressByIdAsync(int id, int userId);
    Task<Address> AddAddressAsync(AddressDto address, int id);
    Task UpdateAddressAsync(AddressDto address, int AddressId, int userid);
    Task DeleteAddressAsync(int AddressId, int userid);
}
