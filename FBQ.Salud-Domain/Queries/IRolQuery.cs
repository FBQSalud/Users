using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Queries
{
    public interface IRolQuery
    {
        Task<User> LoginUser(string email, string password);
    }
}
