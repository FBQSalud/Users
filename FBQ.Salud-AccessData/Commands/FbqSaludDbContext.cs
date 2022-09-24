using FBQ.Salud_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FBQ.Salud_AccessData.Commands
{
    public class FbqSaludDbContext : DbContext
    {
        public FbqSaludDbContext() {}

        public FbqSaludDbContext(DbContextOptions<FbqSaludDbContext> options) : base(options) {}

        public virtual DbSet<User> Users { get; set; }
    }
}
