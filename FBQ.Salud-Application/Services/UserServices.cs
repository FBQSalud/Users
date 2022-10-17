
using AutoMapper;
using FBQ.Salud_Application.Helper;
using FBQ.Salud_Application.Interfaces;
using FBQ.Salud_Application.Models;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Application.Models.Enums;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace FBQ.Salud_Application.Services
{
    public interface IUsersServices
    {
        Task<IEnumerable<User>> GetAll(bool listEntity);
        Task<Response<UserOutDTO>> GetMe();
    }
    public class UsersServices : IUsersServices
    {
        private readonly IEmailServices _emailService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEntityMapper _entityMapper;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersServices(IUserRepository userRepository,
            IMapper mapper,
            IJwtTokenProvider jwtTokenProvider,
            IEntityMapper entityMapper,
            IHttpContextAccessor accessor,
            IConfiguration configuration,
            IEmailServices emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenProvider = jwtTokenProvider;
            _entityMapper = entityMapper;
            _accessor = accessor;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<Response<string>> DeleteUser(int id)
        {
            try
            {
                await _userRepository.DeleteUser(id);
            }
            catch (InvalidOperationException e)
            {
                return new Response<string>("Error", succeeded: false, message: e.Message);
            }
            return new Response<string>("Succes", message: "Entity Deleted");
        }

        public Task<IEnumerable<User>> GetAll(bool listEntity)
        {
            return _userRepository.GetAll(listEntity);
        }

        public async Task<Response<UserOutDTO>> GetMe()
        {
            if (_accessor.HttpContext != null)
            {
                var id = int.Parse(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var entity = await _userRepository.GetById(id);

                var entityDto = _entityMapper.UserToUserOutDTO(entity);

                return new Response<UserOutDTO>(entityDto);
            }

            return null;
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetUsers(bool listEntity)
        {
            try
            {
                var listUserAll = await _userRepository.GetAll(listEntity);

                return listUserAll.Select(user => _entityMapper.UserToUserDto(user)).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Response<string>> InsertUser(RegisterDto registerDto)
        {
            var response = new Response<string>();
            try
            {
                var ListUsers = _userRepository.GetAll(true).Result;
                if (ListUsers != null)
                {
                    if (!Exist(ListUsers, registerDto.Email))
                    {
                        var registeredUser = _entityMapper.RegisterDtoToUser(registerDto);

                        registeredUser.Password = EncryptSha256.Encrypt(registeredUser.Password);

                        registeredUser.RolesId = RoleTypes.Regular;

                        try
                        {
                            var entity = await _userRepository.AddAsync(registeredUser);

                            var user = await _userRepository.GetById(entity.Id, "Roles");

                            var subject = "Confirmación de registro";

                            var body = $"Bienvenido {user.UserName} {user.Picture}";

                            await _emailService.Send(user.Email, _configuration.GetSection("emailContacto").Value, subject, body);

                            var token = _jwtTokenProvider.CreateJwtToken(user);
                            response.Data = token.Result;
                            response.Message = "token return";
                            response.Succeeded = true;
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        response.Message = "Ya existe un usuario con ese email.";
                        response.Succeeded = false;
                    }
                }
                else
                {
                    response.Message = "El campo email no puede quedar vacio.";
                    response.Succeeded = false;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        public async Task<Response<UserOutDTO>> UpdateUserAsync(int id, RegisterDto update)
        {
            try
            {
                var userId = await _userRepository.GetById(id);
                if (userId == null)
                    return new Response<UserOutDTO>(null, false, null, "Entity Not Found");

                var user = _entityMapper.RegisterDtoToUser(update, userId);

                await _userRepository.Update(user);
                var userResponse = _entityMapper.UserToUserOutDTO(user);
                return new Response<UserOutDTO>(userResponse, true, null, "Success!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exist(IEnumerable<User> users, string email)
        {
            var exist = users.Where(user => user.Email == email).FirstOrDefault();
            if (exist != null)
            {
                return true;
            }

            return false;
        }
    }
}
