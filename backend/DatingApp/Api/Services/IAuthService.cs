using System;
using Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services;

public interface IAuthService
{
    Task<ActionResult<UserDto>> CreateUser(RegisterDto registerDto);
    Task<bool> EmailExists(string email);
    Task<ActionResult<UserDto>> LoginUser(LoginDto loginDto);
}
