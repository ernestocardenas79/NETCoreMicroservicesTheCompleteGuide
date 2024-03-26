using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utillity;

namespace Mango.Web.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService= baseService;
    }

    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequesDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.AuthAPIBase + "/api/auth/AssignRole",
            Data = registrationRequesDto,
        });
    }

    public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.AuthAPIBase + "/api/auth/login",
            Data = loginRequestDto,
        });
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequesDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.AuthAPIBase + "/api/auth/register",
            Data = registrationRequesDto,
        });
    }
}
