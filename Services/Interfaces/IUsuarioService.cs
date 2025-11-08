using System.Collections.Generic;
using System.Threading.Tasks;
using MotoTrackAPI.DTO.Usuario.Request;
using MotoTrackAPI.DTO.Usuario.Response;

namespace MotoTrackAPI.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioResponse>> GetAllAsync();
        Task<UsuarioResponse> GetByIdAsync(long id);
        Task<UsuarioResponse> CreateAsync(UsuarioCreateRequest dto);
        Task<UsuarioResponse> UpdateAsync(long id, UsuarioUpdateRequest dto);
        Task DeleteAsync(long id);
    }
}
