
using AutoMapper;
using FBQ.Salud_Application.Helper;
using FBQ.Salud_Application.Interfaces;
using FBQ.Salud_Application.Mapper;
using FBQ.Salud_Application.Models;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using IRolRepository = FBQ.Salud_Domain.Commands.IRolRepository;

namespace FBQ.Salud_Application.Services
{
    public interface IRolServices
    {
        Task<IEnumerable<Rol>> GetRoles(bool listEntity);
        Task<Rol> GetRol(int id);
        Task<Rol> InsertRol(Rol entity);
        Task UpdateRol(int id, Rol entity);
        Task DeleteRol(int id);
        Response<AuthUserDto> LoginUser(LoginDto login);
    }
    public class RolesServices : IRolServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IEntityMapper _entityMapper;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;

        public RolesServices(IJwtTokenProvider jwtTokenProvider, 
            IRolRepository rolRepository,
            IMapper mapper,
            IEntityMapper entityMapper,
            IUserRepository userRepository)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _rolRepository = rolRepository;
            _mapper = mapper;
            _entityMapper = entityMapper;
            _userRepository = userRepository;
        }

        public Task<Rol> GetRol(int id)
        {
            var rol = _userRepository.GetById(id);
            if (rol.Result != null)
            {
                return rol;
            }
            throw new Exception("Rol null");
        }

        public Task<IEnumerable<Rol>> GetRoles(bool listEntity)
        {
            throw new NotImplementedException();
        }

        public Task<Rol> InsertRol(Rol entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRol(int id, Rol entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRol(int id)
        {
            throw new NotImplementedException();
        }

        public Response<AuthUserDto> LoginUser(LoginDto login)
        {
            var response = new Response<AuthUserDto>();

            try
            {
                var user = _rolRepository.GetUserByEmailOrDefault(login);
                if (user.Result != null)
                {
                    if (EncryptSha256.SamePasswords(user.Result.Password, login.Password))
                    {
                        string token = _jwtTokenProvider.CreateJwtToken(user.Result).Result;

                        response.Data = _entityMapper.UserToAuthUserDto(user.Result, token);
                        response.Succeeded = true;
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Succeeded = false;
                        response.Message = "Email o Contraseña incorrecta.";
                    }
                }
                else
                {
                    response.Succeeded = false;
                    response.Message = "El email ingresado no existe.";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
