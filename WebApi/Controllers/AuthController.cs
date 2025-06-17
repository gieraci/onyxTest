using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenSvc;

        public AuthController(TokenService tokenSvc)
        {
            _tokenSvc = tokenSvc;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "user" && request.Password == "pwd")
            {
                var token = _tokenSvc.GenerateToken(request.Username);
                return Ok(new { bearer = token });
            }

            return Unauthorized();
        }
    }

    public record LoginRequest(string Username, string Password);
}
