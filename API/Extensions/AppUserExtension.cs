using System;
using API.DTOs;
using API.Interfaces;
using API.Models;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static UserDto ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
            {
                Id = user.Id,
                DisplayName = user.Name,
                Email = user.Email,
                Token = tokenService.CreatToken(user)
            };
    }
}
