using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.HelseFlow.Application.Features.Auth.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}
