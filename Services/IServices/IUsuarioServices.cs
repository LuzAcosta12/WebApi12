using Domain.DTO;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi29.Services.IServices
{
    public interface IUsuarioServices
    {
        //Read
        Task<Response<List<Usuario>>> ObtenerUsuarios();

        //Read for Id
        Task<Response<Usuario>> ById(int id);

        //Read for UserName
        Task<Response<Usuario>> ByUserName(string userName);

        //Create
        Task<Response<Usuario>> Crear(UsuarioRequest usuario);

        //Update
        Task<Response<Usuario>> Actualizar(int id, UsuarioRequest request);

        //Delete
        Task<Response<string>> Eliminar(int id);
    }
}
