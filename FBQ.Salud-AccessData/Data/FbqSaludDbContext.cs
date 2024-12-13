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
                FechaAlta= DateTime.Now,
                Picture="claudio.jpg",
                SoftDelete= false,
                RolId=1
            }, new User
            {
                UserId = 2,
                UserName = "mariano",
                DNI = "41389372",
                Email = "marianocarrizo@gmail.com",
                Password = "carrizo",
                EmployeeId = 2,
                FechaAlta= DateTime.Now,
                Picture = "foto.jpg",
                SoftDelete = false,
                RolId = 1
            },
             new User
             {
                 UserId = 3,
                 UserName = "medico",
                 DNI = "41389373",
                 Email = "medic@gmail.com",
                 Password = "medic",
                 EmployeeId = 3,
                 FechaAlta= DateTime.Now,
                 Picture = "avatar-01.jpg",
                 SoftDelete = false,
                 RolId = 2
             },
              new User
              {
                  UserId = 4,
                  UserName = "segundez",
                  DNI = "41389376",
                  Email = "segundez@gmail.com",
                  Password = "segundo",
                  EmployeeId = 4,
                  FechaAlta= DateTime.Now,
                  Picture = "avatar-05.jpg",
                  SoftDelete = false,
                  RolId = 2
              },
               new User
               {
                   UserId = 5,
                   UserName = "tercerez",
                   DNI = "41389379",
                   Email = "shrek@gmail.com",
                   Password = "tercero",
                   EmployeeId = 5,
                   FechaAlta= DateTime.Now,
                   Picture = "avatar-03.jpg",
                   SoftDelete = false,
                   RolId = 2
               },
                 new User
                 {
                     UserId = 6,
                     UserName = "clotilde",
                     DNI = "41389379",
                     Email = "krokotilde@gmail.com",
                     Password = "kotil",
                     EmployeeId =6,
                     FechaAlta= DateTime.Now,
                     Picture = "avatar-02.jpg",
                     SoftDelete = false,
                     RolId = 3
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
