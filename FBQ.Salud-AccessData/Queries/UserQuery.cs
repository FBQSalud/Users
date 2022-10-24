using FBQ.Salud_AccessData.Data;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace FBQ.Salud_AccessData.Queries
{
    public class UserQuery : IUserQuery
    {
        private readonly FbqSaludDbContext _context;

        public UserQuery(FbqSaludDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetListUser()
        {
            var users =await (from u in _context.Users where u.SoftDelete == false select u).ToListAsync();
            //var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserByDNIAsync(string UserDNI)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.DNI == UserDNI);
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string UserEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == UserEmail);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId && x.SoftDelete==false);
            return user;
        }
    }
}
