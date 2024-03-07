namespace Mango.Services.AuthAPI.Models.Dto;

public class LoginrResponseDto
{
    public UserDto User { get; set; }
    public string Token { get; set; }
}
