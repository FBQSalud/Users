using FBQ.Salud_AccessData.Data;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace FBQ.Salud_AccessData.Queries
{
    public class RolQuery : IRolQuery
    {
        private readonly FbqSaludDbContext _context;
        public RolQuery(FbqSaludDbContext context)
        {
            _context = context;
        }

        public async Task<User> LoginUser(string email, string password)
        {
            
            var usuario = await _context.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();

            return usuario;

        }
    }
}
