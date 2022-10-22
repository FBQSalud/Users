using FBQ.Salud_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FBQ.Salud_AccessData.Data
{
    public class FbqSaludDbContext : DbContext
    {
        public FbqSaludDbContext() { }

        public FbqSaludDbContext(DbContextOptions<FbqSaludDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                //entity.HasOne<Admin>(x => x.Admin).WithOne(a => a.Rol).HasForeignKey<Admin>(x => x.RolId);
                //entity.HasOne<User>(x => x.User).WithOne(a => a.Rol).HasForeignKey<User>(x => x.RolId);
                entity.HasOne(x => x.Rol).WithMany(a => a.Users).HasForeignKey(x => x.RolId);
            });

            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId= 1,
                UserName="admin",
                DNI="",
                Email="fbq.salud@gmail.com",
                Password="admin",
                EmployeeId= 1,
                Picture="",
                SoftDelete=false,
                RolId=1
            });
            modelBuilder.Entity<Rol>().HasData(
            new Rol
            {
                RolId = 1,
                Name = "Administrador"
            },
            new Rol
            {
                RolId = 2,
                Name = "Medico"
            },
            new Rol
            {
                RolId = 3,
                Name = "Recepcionista"
            });
        }   
    }
}
