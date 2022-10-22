using FBQ.Salud_Domain.Entities;

namespace FBQ.Salud_Domain.Commands
{
    public interface IUserCommand
    {
        Task Update(User user);
        Task Delete(User user);
        Task Add(User user);
    }
}
