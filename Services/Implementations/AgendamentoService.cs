using System; // ✅ para DateTime
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Agendamento.Request;   // ✅ Request
using MotoTrackAPI.DTO.Agendamento.Response;  // ✅ Response
using MotoTrackAPI.Models;
using MotoTrackAPI.Services.Exceptions;
using MotoTrackAPI.Services.Interfaces;

namespace MotoTrackAPI.Services.Implementations
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _db;
        public AgendamentoService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<AgendamentoResponse>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Agendamentos
                .AsNoTracking()
                .Include(a => a.Moto)
                .ToListAsync(ct);

            return list.Select(MapToResponse);
        }

        public async Task<AgendamentoResponse> GetByIdAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Agendamentos
                .AsNoTracking()
                .Include(a => a.Moto)
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (entity is null)
                throw new NotFoundException($"Agendamento {id} não encontrado.");

            return MapToResponse(entity);
        }

        public async Task<AgendamentoResponse> CreateAsync(AgendamentoCreateRequest dto, CancellationToken ct = default)
        {
            // ❗ DTO usa DateTime (não Offset). Use DateTime.Now para comparar.
            if (dto.DataAgendada <= DateTime.Now)
                throw new DomainValidationException("A data agendada deve ser futura.");

            // Moto requerida e deve existir (Create tem MotoId)
            var motoExiste = await _db.Motos.AsNoTracking().AnyAsync(m => m.Id == dto.MotoId, ct);
            if (!motoExiste)
                throw new NotFoundException($"Moto {dto.MotoId} não encontrada.");

            var entity = new Agendamento
            {
                MotoId = dto.MotoId,
                DataAgendada = dto.DataAgendada,
                Descricao = dto.Descricao
            };

            _db.Agendamentos.Add(entity);
            await _db.SaveChangesAsync(ct);

            await _db.Entry(entity).Reference(e => e.Moto).LoadAsync(ct);
            return MapToResponse(entity);
        }

        public async Task<AgendamentoResponse> UpdateAsync(long id, AgendamentoUpdateRequest dto, CancellationToken ct = default)
        {
            var entity = await _db.Agendamentos.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (entity is null)
                throw new NotFoundException($"Agendamento {id} não encontrado.");

            if (dto.DataAgendada <= DateTime.Now)
                throw new DomainValidationException("A data agendada deve ser futura.");

            // ⚠️ UpdateRequest NÃO tem MotoId (não trocamos a moto no update)
            // Se quiser permitir trocar a moto, inclua MotoId em AgendamentoUpdateRequest e revalide aqui.

            entity.DataAgendada = dto.DataAgendada;
            entity.Descricao = dto.Descricao;

            await _db.SaveChangesAsync(ct);

            await _db.Entry(entity).Reference(e => e.Moto).LoadAsync(ct);
            return MapToResponse(entity);
        }

        public async Task DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Agendamentos.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (entity is null)
                throw new NotFoundException($"Agendamento {id} não encontrado.");

            _db.Agendamentos.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        private static AgendamentoResponse MapToResponse(Agendamento a) => new()
        {
            Id = a.Id,
            MotoId = a.MotoId,
            DataAgendada = a.DataAgendada,
            Descricao = a.Descricao,
            DataCriacao = a.DataCriacao
            // ❌ Sem MotoPlaca aqui porque o DTO não tem esse campo
        };
    }
}
