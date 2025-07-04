using System;
using Api.DTOs;
using Api.Services;
using DatingApp.Entities;

namespace Api.ExtensionsMethods;

public static class AppUserExtensions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Id = user.Id,
            Token = tokenService.CreateToken(user)
        };
    }
}
