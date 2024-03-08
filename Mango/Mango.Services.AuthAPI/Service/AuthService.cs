using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _usermanager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> usermanager,
        RoleManager<IdentityRole> roleManager)
    {
        this._db = appDbContext;
        this._usermanager = usermanager;
        this._roleManager = roleManager;
    }

    public Task<LoginrResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.PhoneNumber
        };

        try
        {
            var result = await _usermanager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.ApplicationUsers.First(u=> u.UserName == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    Email = userToReturn.Email,
                    Id = userToReturn.Id,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber
                };

                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception ex)
        {

        }

        return "Some error has ocurred";
    }
}
