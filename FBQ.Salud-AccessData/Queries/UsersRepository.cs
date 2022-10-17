using FBQ.Salud_AccessData.Commands;
using FBQ.Salud_Application.Models;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FBQ.Salud_AccessData.Queries
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
        void Add(User user);
        void Update(User user);
        void AddAsync(User user);
        Task<User> GetById(int id, string include);
    }
    public class UsersRepository : IUserRepository
    {
        private readonly FbqSaludDbContext _context;
        public UsersRepository(FbqSaludDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll(bool listEntity)
        {
            if (!listEntity)
            {
                return await _context.Users.Where(x => x.IsDeleted == true).ToListAsync();
            }

            return await _context.Users.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<User> GetById(int id, string include)
        {
            var entity = await _context.Users
                .Include(include)
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity?.IsDeleted == false ? entity : null;
        }
        public async Task DeleteUser(int id)
        {
            var entity = await GetById(id);

            if (entity == null || entity.IsDeleted == true)
                throw new InvalidOperationException("Entity not found");

            entity.IsDeleted = true;
            _context.Users.Update(entity);
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetUsers(bool listEntity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> InsertUser(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserOutDTO>> UpdateUserAsync(int id, RegisterDto update)
        {
            throw new NotImplementedException();
        }

        Task<Response<string>> IUserRepository.DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void AddAsync(User user)
        {
            _context.Users.AddAsync(user);
            _context.SaveChanges();
        }
    }
}

