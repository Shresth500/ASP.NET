using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class AuthRepo(ApplicationDbContext context,IConfiguration configuration) : IAuthRepo
{
    public async Task<User> checkEmailExists(string Email)
    {
        var data = await context.User.FirstOrDefaultAsync(x => x.Email == Email.ToLower());
        return data!;
    }

    public bool checkPasswordAsync(User user, string password)
    {
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return result == PasswordVerificationResult.Success;
    }

    public int checkRefreshTokenValidity(string RefreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(RefreshToken))
        {
            return 0;
        }
        var token = handler.ReadJwtToken(RefreshToken);
        if(token.ValidTo <= DateTime.Now)
        {
            return 1;
        }
        return 2;
    }



    public async Task<User> createUserAsync(UserDto userData)
    {
        var newUser = new User
        {
            Email = userData.Email.ToLower(),
            Username = userData.Email.ToLower(),
            Role = userData.Role,
        };
        var hashpassword = new PasswordHasher<User>().HashPassword(newUser, userData.Password);
        newUser.PasswordHash = hashpassword;
        await context.User.AddAsync(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> findEmail(string email)
    {
        var data = await context.User.FirstOrDefaultAsync(x => x.Email == email);
        if (data == null)
            return false;
        return true;
    }


    public TokensDto GenerateTokenByRefreshToken(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(refreshToken);
        var id = token.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var email = token.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        var role = token.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
        if (id == null || email == null || role == null)
            return new TokensDto();
        return GenerateTokens(int.Parse(id), email, role);
    }

    public TokensDto GenerateTokens(int id, string email,string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,id.ToString()),
            new Claim(ClaimTypes.Email,email!),
            new Claim(ClaimTypes.Role,role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        var refreshToken = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires:DateTime.Now.AddDays(2),
            signingCredentials:credentials
            );
        //return new JwtSecurityTokenHandler().WriteToken(token);
        return new TokensDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
        };
    }

}
