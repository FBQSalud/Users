using AutoMapper;
using FBQ.Salud_Application.Models;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FBQ.Salud_AccessData.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _service;
        private readonly IMapper _mapper;

        public UserController(IUserRepository service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var listUser = await _service.GetUsers(true);
                return Ok(listUser);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromForm] RegisterDto userUpdate)
        {
            try
            {
                var result = await _service.UpdateUserAsync(id, userUpdate);
                return result.Succeeded == true ? Ok(result) : NotFound(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response<string>(null, false, new string[] { e.Message }, "Server Error"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _service.DeleteUser(id);

                return response.Succeeded == false ? StatusCode(403, response) : Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}        
        

