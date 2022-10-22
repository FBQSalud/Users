using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Application.Validations
{
    public interface IUserValidatorExist
    {
        Task <bool> ExisteUserAsync(User user);
        Task<bool> ExisteEmailAsync(User user);
    }
    public class UserValidatorExist : IUserValidatorExist
    {
        private readonly IUserQuery _query;
        public UserValidatorExist(IUserQuery query)
        {
            _query = query;
        }

        public async Task<bool> ExisteEmailAsync(User user)
        {
            var busquedaCliente = await _query.GetUserByEmailAsync(user.Email);

            return busquedaCliente == null;
        }

        public async Task<bool> ExisteUserAsync(User user)
        {
            var busquedaCliente = await _query.GetUserByDNIAsync(user.DNI);

            return busquedaCliente == null;
        }
    }
}
