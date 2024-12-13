using AutoMapper;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FBQ.Salud_Application.Services
{
    public interface IRolService
    {
        Task<Response> LoginUser(string email, string password);
        Response ValidarToken(ClaimsIdentity identity);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<ClaimsPrincipal> ValidateResetToken(string token);
    }
    public class RolServices : IRolService
    {
        private readonly IRolQuery _rolQuery;
        public readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAdminQuery _adminQuery;
        public RolServices(IRolQuery rolQuery,IConfiguration configuration, IMapper mapper, IAdminQuery adminQuery)
        {
            _rolQuery = rolQuery;
            _configuration = configuration;
            _mapper = mapper;
            _adminQuery = adminQuery;
        }

        public async Task<Response> LoginUser(string email, string password)
        {
            
            var usuario=await _rolQuery.LoginUser(email, password);

            var usuarioMappeado = _mapper.Map<User>(usuario);

            if (usuarioMappeado==null)
            {

                return new Response
                {
                    Success = false,
                    Message = "credenciales incorrectas",
                    Result = ""
                };
            }
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("name", usuario.UserName.ToString()),
                new Claim("id", usuario.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singin = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken
                (
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires : DateTime.Now.AddHours(1),
                    signingCredentials:singin
                );
            return new Response
            {
                Success = true,
                Message = "exito",
                Result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public Response ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Verificar token valido",
                        Result = ""
                    };
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
                
                User admin = _adminQuery.GetAdminById(id);

                return new Response
                {
                    Success = true,
                    Message = "Exito ",
                    Result = admin
                };

            } 
            catch (Exception ex)
            {

                return new Response
                {
                    Success = false,
                    Message = "Catch: " + ex.Message,
                    Result = ""
                };
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            // Validar el usuario
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
            }

            // Configuración del JWT
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            // Claims específicos para el restablecimiento de contraseña
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.UserId.ToString()), // ID del usuario
                new Claim("email", user.Email), // Email del usuario
                new Claim("resetPassword", "true") // Claim para indicar que es un token de restablecimiento
            };

            // Generar la clave y las credenciales de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            // Crear el token
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30), // Token válido por 30 minutos
                signingCredentials: signingCredentials
            );

            // Devolver el token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ClaimsPrincipal> ValidateResetToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true, // Validar que el token no haya expirado
                ClockSkew = TimeSpan.Zero // Sin tolerancia para tiempos
            };

            try
            {
                // Declarar la variable validatedToken
                SecurityToken validatedToken;

                // Validar el token y obtener los claims
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Verificar que el token sea un JWT y contenga el propósito adecuado
                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Claims.Any(c => c.Type == "resetPassword" && c.Value == "true"))
                {
                    return principal;
                }

                throw new SecurityTokenException("El token no es válido para restablecer contraseña.");
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("El token es inválido o ha expirado.", ex);
            }
        }
    }
}
