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
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> usermanager,
        RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        this._db = appDbContext;
        this._usermanager = usermanager;
        this._roleManager = roleManager;
        this._jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) {
                _roleManager.CreateAsync(new (roleName)).GetAwaiter().GetResult();
            }
            
            await _usermanager.AddToRoleAsync(user, roleName);

            return true;
        }

        return false;
    }

    public async Task<LoginrResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u=> u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValid = await _usermanager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (user ==null || !isValid)
        {
            return new LoginrResponseDto() { User=null, Token=""};
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };

        LoginrResponseDto loginrResponseDto = new LoginrResponseDto()
        {
            User = userDto,
            Token = token
        };

        return loginrResponseDto;
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
