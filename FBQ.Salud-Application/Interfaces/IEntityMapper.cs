
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using UserDto = FBQ.Salud_Application.Models.DTOs.UserDto;

namespace FBQ.Salud_Application.Interfaces
{
    public interface IEntityMapper
    {
        UserDto UserToUserDto(User user);

        UserOutDTO UserToUserOutDTO(User user);

        AuthUserDto UserToAuthUserDto(User user, string token);
        User RegisterDtoToUser(RegisterDto registerDto);
        User RegisterDtoToUser(RegisterDto update, User userId);
    }
}
