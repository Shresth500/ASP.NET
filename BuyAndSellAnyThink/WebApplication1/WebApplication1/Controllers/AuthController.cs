using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthRepo repo) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        request.Role = "User";
        var findEmail = await repo.findEmail(request.Email);
        if (findEmail)
            return BadRequest("Email Already exists");
        var data = await repo.createUserAsync(request);
        if (data == null)
            return BadRequest("Something went wrong");
        return Ok("User is registered, pleas login");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto request) 
    {
        var userData = await repo.checkEmailExists(request.Email);
        if (userData == null)
            return NotFound("Email doesn't exist");
        var passwordCheck = repo.checkPasswordAsync(userData, request.Password);
        if (!passwordCheck)
            return BadRequest("Password Incorrect");
        var tokenData = repo.GenerateTokens(userData.Id,userData.Email,userData.Role);
        var data = new LoginResponseDto
        {
            Id = userData.Id,
            Email = request.Email,
            AuthToken = tokenData.AccessToken,
            Role = userData.Role,
            RefreshToken = tokenData.RefreshToken
        };
        return Ok(data);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Token field is required");
        var validity = repo.checkRefreshTokenValidity(refreshToken);
        if (validity == 0)
            return BadRequest("Token Not Found");
        if(validity == 1)
        {
            return Unauthorized("Token is expired");
        }
        var tokensData = repo.GenerateTokenByRefreshToken(refreshToken);
        return Ok(tokensData);
    }

}
