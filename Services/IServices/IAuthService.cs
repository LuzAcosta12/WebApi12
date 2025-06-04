using Domain.DTO;
using Domain.Entities;

namespace WebApi29.Services.IServices
{
    public interface IAuthService
    {
        Task<Response<string>> Login(LoginRequest request);
        string GenerateToken(Usuario usuario);
    }
}
