using System;
using System.Security.Cryptography;
using System.Text;
using Api.DTOs;
using Api.ExtensionsMethods;
using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class AuthServices(DbAppContext context, ITokenService tokenService) : IAuthService
{
    public async Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto)
    {
        if (await EmailExists(registerDto.Email)) return new BadRequestObjectResult("Email already exists");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenService);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<ActionResult<UserDto>> LoginUser(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return new UnauthorizedObjectResult("Invalid email or password");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return new UnauthorizedObjectResult("Invalid email or password");
        }

        return user.ToDto(tokenService);

    }
}
