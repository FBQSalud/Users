
using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Application.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetRoles(bool listEntity);
        Task<Rol> GetRol(int id);
        Task<Rol> InsertRol(Rol entity);
        Task UpdateRol(int id, Rol entity);
        Task DeleteRol(int id);
    }
}
