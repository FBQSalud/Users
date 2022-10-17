
using FBQ.Salud_Application.Models;
using FBQ.Salud_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FBQ.Salud_AccessData.Commands
{
    public class FbqSaludDbContext : DbContext
    {
        public FbqSaludDbContext() {}

        public FbqSaludDbContext(DbContextOptions<FbqSaludDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=DbUsersApi;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
    }
}
