 
using System.ComponentModel.DataAnnotations;


namespace FBQ.Salud_Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DNI { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Password { get; set; }
        public int EmployeeId { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public bool SoftDelete { get; set; } = false;
        public int RolId { get; set; }
        public Rol Rol { get; set; }


    }
}
