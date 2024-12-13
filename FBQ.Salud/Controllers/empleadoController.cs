using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FBQ.Salud_AccessData.Controllers
{
    [Route("api/users")]
    [ApiController]
    // [Authorize]
    public class EmpleadoController : ControllerBase
    {
        private readonly IUserServices _service;
        private readonly IRolService _rolService;

        public EmpleadoController(IUserServices service, IMapper mapper, IRolService rolService)
        {
            _service = service;
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<dynamic> GetAll()
        {
            try
            {
                var users = await _service.GetAll();

                if (users.Count() == 0)
                {

                    return NotFound(users);
                }
                else
                {
                    return Ok(users);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("id")]
        public async Task<dynamic> GetById(int id)
        {
            try
            {
                var user = await _service.GetUserById(id);


                if (user == null)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "empleados con id "+ id +" inexistente",
                        Result = ""
                    };
                }
                else
                    return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
        {
            try
            {
                // Validar modelo recibido
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Intentar crear el usuario
                var userNuevo = await _service.CreateUser(user);

                // Devolver respuesta basada en el resultado del servicio
                if (userNuevo.Success)
                {
                    return CreatedAtAction(nameof(CreateUser), null, userNuevo.Result);
                }
                else
                {
                    return Conflict(new { message = userNuevo.Message }); // Código 409 con mensaje
                }
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                Console.Error.WriteLine(ex);

                // Devolver respuesta genérica de error interno
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        [HttpPut]
        [Route("id")]
        public async Task<dynamic> UpdateUser(int id, UserPut user)
        {
            try
            {           
                var UserResponse = await _service.Update(id, user);

                if (UserResponse.Success)
                {
                    return Ok(UserResponse);
                }
                else
                {
                    return NotFound(UserResponse);
                }
                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("eliminar")]
        public async Task<dynamic> DeleteCliente(int id)
        {
            try
            {
                var user = await _service.Delete(id);

                if (user.Success == false)
                {
                    return new JsonResult(user) { StatusCode = 404 };
                }
                else
                    return Ok(user);
                    //return NoContent();
                  
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}        
        

