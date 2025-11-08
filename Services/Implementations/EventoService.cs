using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Evento.Request;    // ✅ Request
using MotoTrackAPI.DTO.Evento.Response;   // ✅ Response
using MotoTrackAPI.Models;
using MotoTrackAPI.Services.Exceptions;
using MotoTrackAPI.Services.Interfaces;

namespace MotoTrackAPI.Services.Implementations
{
    public class EventoService : IEventoService
    {
        private readonly AppDbContext _db;
        public EventoService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<EventoResponse>> GetAllAsync()
        {
            var list = await _db.Eventos
                .AsNoTracking()
                .Include(e => e.Moto)
                .ToListAsync();

            return list.Select(MapToResponse);
        }

        public async Task<EventoResponse> GetByIdAsync(long id)
        {
            var entity = await _db.Eventos
                .AsNoTracking()
                .Include(e => e.Moto)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity is null)
                throw new NotFoundException($"Evento {id} não encontrado.");

            return MapToResponse(entity);
        }

        public async Task<EventoResponse> CreateAsync(EventoCreateRequest dto)
        {
            // ✅ Valida se a moto existe
            var motoExiste = await _db.Motos.AnyAsync(m => m.Id == dto.MotoId);
            if (!motoExiste)
                throw new NotFoundException($"Moto {dto.MotoId} não encontrada.");

            var entity = new Evento
            {
                MotoId = dto.MotoId,      // ⚠️ Apenas FK (não reatribua entity.Moto)
                Tipo = dto.Tipo,
                Motivo = dto.Motivo,
                DataHora = dto.DataHora,
                Localizacao = dto.Localizacao
            };

            _db.Eventos.Add(entity);
            await _db.SaveChangesAsync();

            // Carrega navegação para resposta
            await _db.Entry(entity).Reference(e => e.Moto).LoadAsync();

            return MapToResponse(entity);
        }

        public async Task<EventoResponse> UpdateAsync(long id, EventoUpdateRequest dto)
        {
            var entity = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException($"Evento {id} não encontrado.");

            // ⚠️ EventoUpdateRequest NÃO possui MotoId -> não alteramos a moto do evento
            entity.Tipo = dto.Tipo;
            entity.Motivo = dto.Motivo;
            entity.DataHora = dto.DataHora;
            entity.Localizacao = dto.Localizacao;

            await _db.SaveChangesAsync();

            // Recarrega navegação para compor o response
            await _db.Entry(entity).Reference(e => e.Moto).LoadAsync();

            return MapToResponse(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
                throw new NotFoundException($"Evento {id} não encontrado.");

            _db.Eventos.Remove(entity);
            await _db.SaveChangesAsync();
        }

        // ==========================
        // Mapper centralizado
        // ==========================
        private static EventoResponse MapToResponse(Evento e) => new()
        {
            Id = e.Id,
            MotoId = e.MotoId,
            Tipo = e.Tipo,
            Motivo = e.Motivo,    // ✅ entidade -> DTO
            DataHora = e.DataHora,
            Localizacao = e.Localizacao
        };
    }
}
