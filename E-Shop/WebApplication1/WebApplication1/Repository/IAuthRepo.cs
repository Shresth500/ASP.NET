using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public interface IAuthRepo
{
    Task<bool> findEmail(string email);
    Task<User> createUserAsync(UserDto userData);
    bool checkPasswordAsync(User user,string password);
    Task<User> checkEmailExists(string Email);
    TokensDto GenerateTokens(int id, string email, string role);
    TokensDto GenerateTokenByRefreshToken(string refreshToken);
    int checkRefreshTokenValidity(string RefreshToken);
}
