using Domain.DTO;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi29.Services.IServices;
using WebApi29.Settings;

namespace WebApi29.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public async Task<Response<string>> Login(LoginRequest request)
        {
            // Validación básica
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return new Response<string>("Usuario y contraseña son obligatorios.");
            }

            // Asumimos que si el username es "Admin", es rol admin, si no, es usuario
            string rol = request.UserName == "Admin" ? "Admin" : "Usuario";

            // Simulamos un usuario (no se consulta BD en este reto)
            var usuario = new Usuario
            {
                PkUsuario = 1,
                UserName = request.UserName,
                Password = request.Password,
                FkRol = rol == "Admin" ? 1 : 2
            };

            // Generar token
            var token = GenerateToken(usuario);

            return new Response<string>(token, "Inicio de sesión exitoso.");
        }

        // ✅ Método requerido por IAuthService
        public string GenerateToken(Usuario usuario)
        {
            string rol = usuario.FkRol == 1 ? "Admin" : "Usuario";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.PkUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // (Opcional) Puedes mantener el método privado si lo necesitas en el login simulado
        private string GenerateToken(Usuario usuario, string rol)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.PkUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
