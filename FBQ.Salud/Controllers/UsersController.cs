using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FBQ.Salud_AccessData.Controllers
{
    public class UsersController
    {
        [Route("api/users")]
        [ApiController]       
        public class UserController : ControllerBase
        {
            private readonly IUserServices _service;
            private readonly IMapper _mapper;

            public UserController(IUserServices service, IMapper mapper)
            {
                _service = service;
                _mapper = mapper;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                try
                {
                    var user = _service.GetAll();
                    var userMapped = _mapper.Map<List<UserDto>>(user);

                    return Ok(userMapped);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(int id)
            {
                try
                {
                    var user = _service.GetUserById(id);
                    var userMapped = _mapper.Map<UserDto>(user);
                    if (user == null)
                    {
                        return NotFound("Usuario Inexistente");
                    }
                    return Ok(userMapped);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpPost]
            public IActionResult CreateUser([FromForm] UserDto user)
            {
                try
                {
                    var userEntity = _service.CreateUser(user);

                    if (userEntity != null)
                    {
                        var UserCreated = _mapper.Map<UserDto>(userEntity);
                        return Ok("User Created");
                    }

                    return BadRequest();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpPut("{id}")]
            public IActionResult UpdateUser(int id, UserDto user)
            {
                try
                {
                    if (user == null)
                    {
                        return BadRequest("Completar todos los campos para realizar la actualizacion");
                    }

                    var userUpdate = _service.GetUserById(id);

                    if (userUpdate == null)
                    {
                        return NotFound("Usuario Inexistente");
                    }

                    _mapper.Map(user, userUpdate);
                    _service.Update(userUpdate);

                    return Ok("Usuario actualizado");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteUser(int id)
            {
                try
                {
                    var user = _service.GetUserById(id);

                    if (user == null)
                    {
                        return NotFound("Usuario Inexistente");
                    }

                    _service.Delete(user);
                    return Ok("Usuario eliminado");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
