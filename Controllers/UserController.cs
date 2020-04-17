using MheanMaa.Enum;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MheanMaa.Util.ClaimSearch;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AuthenticationService _authenticationService;

        public UserController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLogin user)
        {
            UserReturn userGet = _authenticationService.Authenticate(user.Username, user.Password);

            if (userGet == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(userGet);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { id = GetClaim(User, ClaimEnum.Id) });
        }
    }
}