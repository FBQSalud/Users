
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Domain.Dtos
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string DNI { get; set; }
        [Required]
        public string Password { get; set; }
        public string Picture { get; set; }
        [Required]
        public int EmployeeId { get; set; } 
        [StringLength(30)]
        public string Email { get; set; }
        public int RolId { get; set; }
    }
}
