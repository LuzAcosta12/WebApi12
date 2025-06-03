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

        public string GenerateToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.PkUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, usuario.Roles?.Nombre ?? "Usuario") // Asegúrate que usuario.Roles no sea null y tenga Nombre
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

        public async Task<Response<string>> Login(LoginRequest request)
        {
            // Validar que UserName y Password no sean nulos o vacíos
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return new Response<string>("El usuario y contraseña son obligatorios");
            }

            // TODO: Aquí debes validar el usuario en la base de datos
            // Por ahora simulamos un usuario válido
            var usuario = new Usuario
            {
                PkUsuario = 1,
                UserName = request.UserName,
                Roles = new Domain.Entities.Rol { Nombre = "Usuario" } // O el rol que corresponda
            };

            // Generar token
            var token = GenerateToken(usuario);

            return new Response<string>(token, "Inicio de sesión exitoso");
        }
    }
}
