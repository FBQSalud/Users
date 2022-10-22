using FBQ.Salud_AccessData.Data;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_AccessData.Queries
{
    public class AdminQuery : IAdminQuery
    {
        private readonly FbqSaludDbContext _context;
        public AdminQuery(FbqSaludDbContext context)
        {
            _context = context;
        }
        public User GetAdminById(string AdminId)
        {
            return _context.Users.FirstOrDefault(x => x.UserId.ToString() == AdminId);
        }
    }
}
