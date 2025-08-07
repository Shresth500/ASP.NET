using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAddressRepo
{
    Task<PagedResult<Address>> findAddressAsync(PaginationQuerDto paginationQuerDto, int userId,CancellationToken token);
    Task<Address> findAddressByIdAsync(int id, int userId,CancellationToken token);
    Task<Address> AddAddressAsync(AddressDto address, int id, CancellationToken token);
    Task UpdateAddressAsync(AddressDto address, int AddressId, int userid,CancellationToken token);
    Task DeleteAddressAsync(int AddressId, int userid, CancellationToken token);
}
