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

        // ==========================================
        // Login (público)
        // ==========================================
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuarioResponse = await _usuarioServices.ByUserName(request.UserName);

            if (!usuarioResponse.Succeded || usuarioResponse.Result == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var usuario = usuarioResponse.Result;

            // Validar contraseña
            if (usuario.Password != request.Password)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var token = _authService.GenerateToken(usuario);
            return Ok(new { token });
        }

        // ==========================================
        // Obtener todos los usuarios (requiere token)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioServices.ObtenerUsuarios();
            return Ok(response);
        }

        // ==========================================
        // Obtener un usuario por ID (requiere token)
        // ==========================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _usuarioServices.ById(id);
            return Ok(response);
        }

        // ==========================================
        // Crear usuario (solo Admin)
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostUser(UsuarioRequest request)
        {
            var response = await _usuarioServices.Crear(request);
            return Ok(response);
        }

        // ==========================================
        // Actualizar usuario (solo Admin)
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutUser(int id, UsuarioRequest request)
        {
            var response = await _usuarioServices.Actualizar(id, request);
            return Ok(response);
        }

        // ==========================================
        // Eliminar usuario (solo Admin)
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _usuarioServices.Eliminar(id);
            return Ok(response);
        }
    }
}
