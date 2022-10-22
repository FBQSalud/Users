using FBQ.Salud_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
