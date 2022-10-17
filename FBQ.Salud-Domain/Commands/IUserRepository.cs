using FBQ.Salud_Application.Models;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Commands
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsers(bool listEntity);
        Task<User> GetUser(int id);
        Task<Response<string>> InsertUser(RegisterDto registerDto);
        Task<Response<UserOutDTO>> UpdateUserAsync(int id, RegisterDto update);
        Task<Response<string>> DeleteUser(int id);
        Task<IEnumerable<User>> GetAll(bool listEntity);
        Task<User> GetById(int id);
        Task<User> Update(User user);
        Task<User> Add(User user);
        Task<User> AddAsync(User user);
        Task<User> GetById(int id, string include);
        Task<Response<UserOutDTO>> GetMe();
    }
}