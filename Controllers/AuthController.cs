using Microsoft.AspNetCore.Mvc;
using WebApi29.Services.IServices;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WebApi29.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // LOGIN — público
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request);

            if (!response.Succeded)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
