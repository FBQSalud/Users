using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Queries
{
    public interface IAdminQuery
    {
        User GetAdminById(string AdminId);
    }
}
