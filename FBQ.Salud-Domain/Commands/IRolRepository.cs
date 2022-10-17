using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Commands
{
    public interface IRolRepository
    {
        Task<User> GetUserByEmailOrDefault(LoginDto login);
        Task<User> GetUserByEmailOrDefault(string email);
        Task<IEnumerable<Rol>> GetRoles(bool listEntity);
        Task<Rol> GetRol(int id);
        Task<Rol> InsertRol(Rol entity);
        Task UpdateRol(int id, Rol entity);
        Task DeleteRol(int id);
    }
}
