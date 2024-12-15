using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FBQ.Salud_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class adminController : ControllerBase
    {
        public readonly IRolService _rolService;
        private readonly IUserServices _userService;
        private readonly IMapper _mapper;
        public adminController(IRolService rolService, IUserServices userService, IMapper mapper)
        {
            _rolService = rolService;
            _userService = userService;
            _mapper = mapper;   
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AdminRequest optData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(optData.Email) || string.IsNullOrWhiteSpace(optData.Password))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El email y la contraseña son obligatorios"
                    });
                }

                var loginResponse = await _rolService.LoginUser(optData.Email, optData.Password);

                if (!loginResponse.Success)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Usuario o contraseña incorrectos"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Inicio de sesión exitoso",
                    token = loginResponse.Result 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Ocurrió un error en el servidor",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest(new { message = "El correo electrónico es obligatorio." });
                }

                var userResponse = await _userService.GetUserByEmailAsync(request.Email);
                if (userResponse == null)
                {
                    return NotFound(new { message = "No se encontró una cuenta asociada a este correo." });
                }

                var userEntity = _mapper.Map<User>(userResponse);

                var resetToken = await _rolService.GeneratePasswordResetTokenAsync(userEntity);

                return Ok(new
                {
                    message = "Token generado exitosamente.",
                    resetToken = resetToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hubo un error al procesar tu solicitud: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                {
                    return BadRequest(new { message = "El token y la nueva contraseña son obligatorios." });
                }

                var claimsPrincipal = await _rolService.ValidateResetToken(request.Token);

                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { message = "El token es inválido o ha expirado." });
                }
                var user = await _userService.GetUserById(int.Parse(userId));
                if (user == null)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }

                var userPut = new UserPut
                {
                    Password = PasswordUtils.HashPassword(request.NewPassword) 
                };

                await _userService.Update(user.UserId, userPut);

                return Ok(new { message = "Contraseña restablecida exitosamente." });
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { message = $"El token es inválido o ha expirado: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Hubo un error al procesar tu solicitud: {ex.Message}" });
            }
        }
    }
}
