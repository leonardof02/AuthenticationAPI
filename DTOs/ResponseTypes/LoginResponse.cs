namespace MiddleApi.DTOs;

public class LoginResponse
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}