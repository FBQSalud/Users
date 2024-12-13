using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Queries
{
    public interface IUserQuery
    {
        Task<List<User>> GetListUser();
        Task<User> GetUserByDNIAsync(string UserDNI);
        Task<User> GetUserByIdAsync(int UserId);
        Task<User> GetUserByEmailAsync(string UserEmail);
    }
}
