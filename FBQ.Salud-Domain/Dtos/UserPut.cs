using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Domain.Dtos
{
    public class UserPut
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Picture { get; set; }
        [StringLength(30)]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
