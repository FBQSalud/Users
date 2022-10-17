
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FBQ.Salud_Application.Models.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El campo Email es requerido.")]
        [StringLength(320, ErrorMessage = "Alcanzo la maxima cantidad de caracteres.")]
        [EmailAddress(ErrorMessage = "El campo de Email no es una dirección de correo electrónico válida.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [StringLength(20, ErrorMessage = "Alcanzo la maxima cantidad de caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Picture { get; set; }
    }
}
