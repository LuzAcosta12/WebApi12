using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi29.Services.IServices;
using Microsoft.AspNetCore.Authorization;


namespace WebApi29.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IAuthService _authService;

        public UsuarioController(IUsuarioServices usuarioServices, IAuthService authService)
        {
            _usuarioServices = usuarioServices;
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _usuarioServices.ByUserName(request.UserName);

            if (!response.Succeded || response.Result == null || response.Result.Password != request.Password)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var token = _authService.GenerateToken(response.Result);
            return Ok(new { token });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioServices.ObtenerUsuarios();
            return Ok(response);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _usuarioServices.ById(id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostUser(UsuarioRequest request)
        {
            var response = await _usuarioServices.Crear(request);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutUser(int id, UsuarioRequest request)
        {
            var response = await _usuarioServices.Actualizar(id, request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _usuarioServices.Eliminar(id);
            return Ok(response);
        }
    }
}
