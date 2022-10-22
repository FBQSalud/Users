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
    }
}
