using AutoMapper;
using FBQ.Salud_Application.Services;
using Microsoft.AspNetCore.Mvc;
using FBQ.Salud_Domain.Dtos;
using FBQ.Salud_Application.Models;
using FBQ.Salud_Application.Models.DTOs;
using FBQ.Salud_Domain.Commands;

namespace FBQ.Salud_Presentation.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userService;
        public IConfiguration _configuration;
        private readonly IRolServices _rolService;
        private readonly IMapper _mapper;

        public AdminController(IRolServices rolService,
            IMapper mapper, 
            IConfiguration configuration,
            IUserRepository userService)
        {
            _rolService = rolService;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromForm] LoginDto login)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var userLogged = _rolService.LoginUser(login);
                if (userLogged == null)
                {
                    return NotFound(userLogged);
                }
                return Ok(userLogged);
            }
            catch (Exception ex)
            {
                var listError = new string[] { ex.Message };
                return StatusCode(500, new Response<String>(data: null, succeeded: false, errors: listError, message: "Error"));
            }
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromForm] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var userRegisteredResult =  await _userService.InsertUser(registerDto);
                if (!(userRegisteredResult.Succeeded))
                {
                    return BadRequest(userRegisteredResult);
                }
                return Ok(userRegisteredResult);
            }
            catch (Exception ex)
            {
                var listError = new string[] { ex.Message };
                return StatusCode(500, new Response<string>(data: null, succeeded: false, errors: listError, message: "Error"));
            }
        }

        [HttpGet]
        [Route("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = _userService.GetMe();
                return Ok(response);
            }
            catch (Exception e)
            {
                var listErrors = new string[]
                {
                    e.Message
                };

                return BadRequest(new Response<UserOutDTO>(null, succeeded: false, listErrors, message: "User not logged"));
            }
        }
    }
}
