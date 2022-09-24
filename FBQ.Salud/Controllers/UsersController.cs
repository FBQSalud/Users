using AutoMapper;
using FBQ.Salud_Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FBQ.Salud_AccessData.Controllers
{
    public class UsersController
    {
        [ApiController]
        [Route("api/users")]
        public class UserController : ControllerBase
        {
            private readonly IUsersServices _service;
            private readonly IMapper _mapper;

            public UserController(IUsersService service, IMapper mapper)
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
                    var usuario = _service.GetUserById(id);
                    var usuarioMapped = _mapper.Map<UserDto>(usuario);
                    if (usuario == null)
                    {
                        return NotFound("Usuario Inexistente");
                    }
                    return Ok(usuarioMapped);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpPost]
            public IActionResult CreateUser([FromForm] UserDto usuario)
            {
                try
                {
                    var usuarioEntidad = _service.CreateUser(usuario);

                    if (usuarioEntidad != null)
                    {
                        var userCreado = _mapper.Map<UserDto>(usuarioEntidad);
                        return Ok("Usuario creado");
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
                    var usuario = _service.GetUserById(id);

                    if (usuario == null)
                    {
                        return NotFound("Usuario Inexistente");
                    }

                    _service.Delete(usuario);
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
