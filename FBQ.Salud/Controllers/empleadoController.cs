using AutoMapper;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FBQ.Salud_AccessData.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class empleadoController : ControllerBase
    {
        private readonly IUserServices _service;
        private readonly IRolService _rolService;

        public empleadoController(IUserServices service, IMapper mapper, IRolService rolService)
        {
            _service = service;
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<dynamic> GetAll()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = _rolService.ValidarToken(identity);

                if (!rToken.Success) return rToken;

                User admin = (User)rToken.Result;

                if (admin.RolId != 1)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "No tienes permiso para ver la lista de empleados ",
                        Result = ""
                    };
                }

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
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = _rolService.ValidarToken(identity);

                if (!rToken.Success) return rToken;

                User admin = (User)rToken.Result;

                if (admin.RolId != 1)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "No tienes permiso para buscar empleados ",
                        Result = ""
                    };
                }
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
        public async Task<dynamic> CreateUser([FromForm] UserRequest user)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = _rolService.ValidarToken(identity);

                if (!rToken.Success) return rToken;

                User admin = (User)rToken.Result;

                if (admin.RolId != 1)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "No tienes permiso para crear empleados ",
                        Result = ""
                    };
                }

                var userNuevo = await _service.CreateUser(user);

                if (userNuevo.Success)
                {
                    return new JsonResult(userNuevo) { StatusCode = 201 };
                }
                else
                {
                    return new JsonResult(userNuevo) { StatusCode = 409 };
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("id")]
        public async Task<dynamic> UpdateUser(int id, UserPut user)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = _rolService.ValidarToken(identity);

                if (!rToken.Success) return rToken;

                User admin = (User)rToken.Result;

                if (admin.RolId != 1)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "No tienes permiso para modificar empleados ",
                        Result = ""
                    };
                }

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
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var rToken = _rolService.ValidarToken(identity);

                if (!rToken.Success) return rToken;

                User admin = (User)rToken.Result;

                if (admin.RolId != 1)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "No tienes permiso para eliminar empleados ",
                        Result = ""
                    };
                }


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
        

