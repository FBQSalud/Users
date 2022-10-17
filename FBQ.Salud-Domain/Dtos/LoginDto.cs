
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [StringLength(320, ErrorMessage = "Alcanzo la maxima cantidad de caracteres.")]
        [EmailAddress(ErrorMessage = "El campo de Email no es una dirección de correo electrónico válida.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [StringLength(20, ErrorMessage = "Alcanzo la maxima cantidad de caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(20, ErrorMessage = "Alcanzo la maxima cantidad de caracteres.")]
        public int RolesId { get; set; }
    }
}
