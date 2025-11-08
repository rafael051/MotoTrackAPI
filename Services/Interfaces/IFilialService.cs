using System.Collections.Generic;
using System.Threading.Tasks;
using MotoTrackAPI.DTO.Filial.Request;
using MotoTrackAPI.DTO.Filial.Response;

namespace MotoTrackAPI.Services.Interfaces
{
    public interface IFilialService
    {
        Task<IEnumerable<FilialResponse>> GetAllAsync();
        Task<FilialResponse> GetByIdAsync(long id);
        Task<FilialResponse> CreateAsync(FilialCreateRequest dto);
        Task<FilialResponse> UpdateAsync(long id, FilialUpdateRequest dto);
        Task DeleteAsync(long id);
    }
}
