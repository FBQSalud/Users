
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace FBQ.Salud_Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
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
        public int RolesId { get; set; }
        [Required]
        public DateTime ModifiedAt { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public Rol Roles { get; set; }
    }
}
