
using AutoMapper;
using FBQ.Salud_Application.Validations;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using FBQ.Salud_Domain.Queries;

namespace FBQ.Salud_Application.Services
{
    public interface IUserServices
    { 
        Task<List<UserResponse>> GetAll();
        Task<UserResponse> GetUserById(int id);
        Task<Response> Update(int id, UserPut user);
        Task<Response>Delete(int userId);
        Task<Response> CreateUser(UserRequest user);
    }
    public class UsersServices : IUserServices
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        private readonly IMapper _mapper;
        private readonly IUserValidatorExist _userValidation;
        public UsersServices(IUserCommand userCommand,IUserQuery userQuery,
            IMapper mapper,IUserValidatorExist userValidation)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
            _mapper = mapper;
            _userValidation = userValidation;
        }
        public async Task<List<UserResponse>> GetAll()
        {
            var users = await _userQuery.GetListUser();

            var usersMapeados = _mapper.Map<List<UserResponse>>(users);

            return usersMapeados;
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _userQuery.GetUserByIdAsync(id);

            var userMappeado = _mapper.Map<UserResponse>(user);

            return userMappeado;
        }

        public async Task<Response> CreateUser(UserRequest user)
        {
            var userMapped = _mapper.Map<User>(user);

            if ((await _userValidation.ExisteUserAsync(userMapped)) && (await _userValidation.ExisteEmailAsync(userMapped)))
            {
                await _userCommand.Add(userMapped);
                return new Response
                {
                    Success = true,
                    Message = "Exito",
                    Result = user
                };
            }
            else
            {

                var userExistente = await _userQuery.GetUserByDNIAsync(userMapped.DNI);

                if (userExistente.SoftDelete==true)
                {
                    userExistente.SoftDelete = false;
                    await _userCommand.Update(userExistente);

                    var userMap = _mapper.Map<UserResponse>(userExistente);
                    return new Response
                    {
                        Success = true,
                        Message = "Empleado con dni " + user.DNI + " activado",
                        Result = userMap
                    };
                }
                else
                    return new Response
                    {
                        Success = false,
                        Message = "Existe un empleado con dni " + user.DNI + " o email "+ user.Email,
                        Result = ""
                    };
            }
        }

        public async Task<Response> Update(int id, UserPut user)
        {
            var userUpdate = await _userQuery.GetUserByIdAsync(id);

            var userMapped = _mapper.Map<User>(user);

            if (userUpdate!=null && await _userValidation.ExisteUserAsync(userMapped))
            {
                _mapper.Map(user, userUpdate);

                await _userCommand.Update(userUpdate);


                return new Response
                {
                    Success = true,
                    Message = "empleado modificado",
                    Result = user
                };
            }
            else
            {

                if (await _userValidation.ExisteUserAsync(userMapped) || await _userValidation.ExisteEmailAsync(userMapped))
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Usuario con dni o email existente",
                        Result = ""
                    };
                }
                return new Response
                {
                    Success = false,
                    Message = "empleado con id " + id + " inexistente",
                    Result = ""
                };
            }
        }

        public async Task<Response> Delete(int userId)
        {
            var user = await _userQuery.GetUserByIdAsync(userId);

            if (user!=null)
            {
                if (user.SoftDelete==true)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Usuario inexistente",
                        Result = ""
                    };
                }
                else
                {
                    user.SoftDelete = true;

                    await _userCommand.Update(user);

                    var userMappeo = _mapper.Map<UserResponse>(user);

                    return new Response
                    {
                        Success = true,
                        Message = "Usuario eliminado",
                        Result = userMappeo
                    };
                }
            }
            else
            {
                return new Response
                {
                    Success = false,
                    Message = "Empleado con id " + userId + " inexistente",
                    Result = " "
                };
            }
            
        }
    }
}
