using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAuthRepo
{
    Task<bool> findEmail(string email,CancellationToken token);
    Task<User> createUserAsync(UserDto userData,CancellationToken token);
    bool checkPasswordAsync(User user,string password);
    Task<User> checkEmailExists(string Email, CancellationToken token);
    TokensDto GenerateTokens(int id, string email, string role);
    TokensDto GenerateTokenByRefreshToken(string refreshToken);
    int checkRefreshTokenValidity(string RefreshToken);
}
