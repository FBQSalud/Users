
using FBQ.Salud_AccessData.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FBQ.Salud_AccessData.Queries
{
    public interface IRolRepository 
    {
        Task<User> GetUserByEmailOrDefault(LoginDto login);
        Task<User> GetUserByEmailOrDefault(string email);
        Task<IEnumerable<Rol>> GetRoles(bool listEntity);
        Task<Rol> InsertRol(Rol entity);
        Task UpdateRol(int id, Rol entity);
        Task DeleteRol(int id);
    }
    public class RolRepository : IRolRepository
    {
        private readonly FbqSaludDbContext _context;
        public RolRepository(FbqSaludDbContext context)
        {
            _context = context;
        }

        public Task DeleteRol(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rol>> GetRoles(bool listEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailOrDefault(LoginDto login)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email && x.IsDeleted == false);
        }

        public async Task<User> GetUserByEmailOrDefault(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<Rol> InsertRol(Rol entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRol(int id, Rol entity)
        {
            throw new NotImplementedException();
        }
    }
}
