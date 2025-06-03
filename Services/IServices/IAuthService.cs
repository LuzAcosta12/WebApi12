using Domain.DTO;
using Domain.Entities;

namespace WebApi29.Services.IServices
{
    public interface IAuthService
    {
        Task<Response<string>> Login(LoginRequest request);  // ← Este es el que falta
        string GenerateToken(Usuario usuario);               // Ya lo tienes
    }
}
