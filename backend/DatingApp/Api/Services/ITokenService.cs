using System;
using DatingApp.Entities;

namespace Api.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
