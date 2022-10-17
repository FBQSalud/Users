
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Domain.Dtos
{
    public class AuthUserDto
    {
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
