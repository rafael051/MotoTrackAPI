using System.Collections.Generic;
using System.Threading.Tasks;
using MotoTrackAPI.DTO.Evento.Request;
using MotoTrackAPI.DTO.Evento.Response;

namespace MotoTrackAPI.Services.Interfaces
{
    public interface IEventoService
    {
        Task<IEnumerable<EventoResponse>> GetAllAsync();
        Task<EventoResponse> GetByIdAsync(long id);
        Task<EventoResponse> CreateAsync(EventoCreateRequest dto);
        Task<EventoResponse> UpdateAsync(long id, EventoUpdateRequest dto);
        Task DeleteAsync(long id);
    }
}
