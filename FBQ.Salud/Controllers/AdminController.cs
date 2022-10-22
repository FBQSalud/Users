using FBQ.Salud_Application.Services;
using FBQ.Salud_Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FBQ.Salud_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class adminController : ControllerBase
    {
        public readonly IRolService _rolService;
        public adminController(IRolService rolService)
        {
            _rolService=rolService;
        }

        [HttpPost]
        [Route("login")]
        public async Task <IActionResult> Login([FromBody]AdminRequest optData)
        {
            try
            {

                string email = optData.Email.ToString();

                string password = optData.Password.ToString();

                var loginn = await _rolService.LoginUser(email, password);

                if (loginn.Success == false)
                {
                    return NotFound(loginn);

                }
                    return Ok(loginn);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
