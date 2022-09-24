using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Commands
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetUserById(int id);
        void Update(User user);
        void Delete(User user);
        void Add(User user);
    }
}
