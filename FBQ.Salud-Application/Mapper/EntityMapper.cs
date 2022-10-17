
using FBQ.Salud_Application.Interfaces;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using UserDto = FBQ.Salud_Application.Models.DTOs.UserDto;

namespace FBQ.Salud_Application.Mapper
{
    public class EntityMapper : IEntityMapper
    {
        public UserDto UserToUserDto(User user)
        {
            var userDto = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Picture = user.Picture,
                RolesId = user.RolesId
            };

            return userDto;
        }

        public AuthUserDto UserToAuthUserDto(User user, string token)
        {
            var _authUserDto = new AuthUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };

            return _authUserDto;
        }

        public User RegisterDtoToUser(RegisterDto update, User user)
        {
            user.UserName = update.UserName != null ? update.UserName : user.UserName;
            user.Email = update.Email != null ? update.Email : user.Email;
            user.Password = update.Password != null ? update.Password : user.Password;
            user.Picture = update.Picture != null ? update.Picture : user.Picture;
            user.ModifiedAt = DateTime.Now;

            return user;
        }

        public User RegisterDtoToUser(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Picture = registerDto.Picture != null ? registerDto.Picture : null,
                ModifiedAt = DateTime.Now
            };
            return user;
        }

        public UserOutDTO UserToUserOutDTO(User user)
        {
            return new UserOutDTO()
            {
                UserName = user.UserName,
                Email = user.Email,
                Picture = user.Picture,
            };
        }
    }
}
