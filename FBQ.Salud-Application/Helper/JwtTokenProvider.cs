
using FBQ.Salud_Application.Interfaces;
using FBQ.Salud_Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FBQ.Salud_Application.Helper
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly IRolRepository _rolRepository;
        public JwtTokenProvider(IConfiguration configuration, 
            IRolRepository rolRepository)
        {
            Configuration = configuration;
            _rolRepository = rolRepository;
        }

        public IConfiguration Configuration { get; }

        public async Task<string> CreateJwtToken(User user)
        {
            var userRoles = await _rolRepository.GetRol(user.RolesId);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRoles.Name)
            };

            var authSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Configuration.GetSection("AppSettings:DevelopmentJwtApiKey").Value));

            var tPayLoad = new JwtSecurityToken(

                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: authClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tPayLoad);


            return token;
        }
    }
}
