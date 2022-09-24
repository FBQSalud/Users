
using System.ComponentModel.DataAnnotations;


namespace FBQ.Salud_Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Status { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        public string TypeOfEmployee { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        public bool SoftDelete { get; set; } = false;

    }
}
