namespace WebApplication1.DTO;

public class LoginResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role {  get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string RefreshToken {  get; set; } = string.Empty;
}
