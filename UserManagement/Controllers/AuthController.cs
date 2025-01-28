using Microsoft.AspNetCore.Mvc;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.EmailId) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid emailid or password.");
            }

            var token = _authService.Authenticate(loginRequest.EmailId, loginRequest.Password);
            if (token == null)
            {
                return Unauthorized("Invalid emailid or password.");
            }

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
