using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Exception = System.Exception;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new ApplicationUser()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.Phonenumber

        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);
                UserDto userDto = new UserDto()
                {
                  Email = userToReturn.Email,
                  ID = userToReturn.Id,
                  Name = userToReturn.Name,
                  Phonenumber = userToReturn.PhoneNumber
                };
                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception e)
        {
           
        }

        return "Error Encountered";
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user =  _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Username.ToLower());
        bool passwordIsValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || passwordIsValid == false )
        {
            return new LoginResponseDto()
            {
              User = null,
              Token = ""
            };
        }
         //if user found, generate JWT Token
        var token = _jwtTokenGenerator.GenerateToken(user);

        UserDto userDto = new UserDto()
        {
          Email = user.Email,
          ID = user.Id,
          Name = user.Name,
          Phonenumber = user.PhoneNumber
        };

        LoginResponseDto loginResponseDto = new LoginResponseDto()
        {
           User =  userDto,
           Token = token
        };
        return loginResponseDto;
    }

    public async Task<bool> AssignRole(string email, string roleName)
    {
        var user =  _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        if (user != null)
        {
          // GetAwaiter().GetResult(): This method blocks the current thread until the asynchronous operation is complete.
          // await: This keyword allows the method to asynchronously wait for the completion of the task without blocking the thread.
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                //create role if it does not exist
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult(); 

            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
        return false;

    }
}