

using FBQ.Salud_Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Application.Models.DTOs
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public string TypeOfEmployee { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        public int RolesId { get; set; }
    }
}
