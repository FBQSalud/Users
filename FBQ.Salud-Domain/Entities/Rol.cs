
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Domain.Entities
{
    public class Rol 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        public DateTime ModifiedAt { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
