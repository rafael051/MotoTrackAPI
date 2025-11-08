using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Moto.Request;    // ✅ Request
using MotoTrackAPI.DTO.Moto.Response;   // ✅ Response
using MotoTrackAPI.Models;
using MotoTrackAPI.Services.Exceptions;
using MotoTrackAPI.Services.Interfaces;

namespace MotoTrackAPI.Services.Implementations
{
    public class MotoService : IMotoService
    {
        private readonly AppDbContext _db;
        public MotoService(AppDbContext db) => _db = db;

        // ===============================
        // GET: todos
        // ===============================
        public async Task<IEnumerable<MotoResponse>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Motos
                .AsNoTracking()
                .Include(m => m.Filial)
                .ToListAsync(ct);

            return list.Select(MapToResponse);
        }

        // ===============================
        // GET: por id
        // ===============================
        public async Task<MotoResponse> GetByIdAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Motos
                .AsNoTracking()
                .Include(m => m.Filial)
                .FirstOrDefaultAsync(m => m.Id == id, ct);

            if (entity is null)
                throw new NotFoundException($"Moto {id} não encontrada.");

            return MapToResponse(entity);
        }

        // ===============================
        // POST: criar
        // ===============================
        public async Task<MotoResponse> CreateAsync(MotoCreateRequest dto, CancellationToken ct = default)
        {
            // 🔧 Normaliza placa (trim, remove hífen, upper)
            var placaNorm = NormalizePlaca(dto.Placa);

            // 📛 Placa única (case-insensitive)
            var placaDup = await _db.Motos
                .AsNoTracking()
                .AnyAsync(m => m.Placa.ToUpper() == placaNorm.ToUpper(), ct);

            if (placaDup)
                throw new ConflictException($"Placa já cadastrada: {dto.Placa}.");

            // 🏢 Filial opcional
            if (dto.FilialId.HasValue)
            {
                var filialExists = await _db.Filiais
                    .AsNoTracking()
                    .AnyAsync(f => f.Id == dto.FilialId.Value, ct);

                if (!filialExists)
                    throw new NotFoundException($"Filial {dto.FilialId} não encontrada.");
            }

            var entity = new Moto
            {
                Placa = placaNorm,
                Modelo = dto.Modelo,
                Marca = dto.Marca,
                Ano = dto.Ano,
                Status = dto.Status,
                FilialId = dto.FilialId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            _db.Motos.Add(entity);
            await _db.SaveChangesAsync(ct);

            // Carrega a navegação para o response
            await _db.Entry(entity).Reference(e => e.Filial).LoadAsync(ct);
            return MapToResponse(entity);
        }

        // ===============================
        // PUT: atualizar
        // ===============================
        public async Task<MotoResponse> UpdateAsync(long id, MotoUpdateRequest dto, CancellationToken ct = default)
        {
            var entity = await _db.Motos.FirstOrDefaultAsync(m => m.Id == id, ct);
            if (entity is null)
                throw new NotFoundException($"Moto {id} não encontrada.");

            // 🔧 Normaliza placa recebida
            var placaNorm = NormalizePlaca(dto.Placa);

            // 📛 Placa única (desconsidera a própria moto), case-insensitive
            var conflito = await _db.Motos
                .AsNoTracking()
                .AnyAsync(m => m.Id != id && m.Placa.ToUpper() == placaNorm.ToUpper(), ct);

            if (conflito)
                throw new ConflictException($"Placa {dto.Placa} já está em uso.");

            // 🏢 Filial opcional
            if (dto.FilialId.HasValue)
            {
                var filialExists = await _db.Filiais
                    .AsNoTracking()
                    .AnyAsync(f => f.Id == dto.FilialId.Value, ct);

                if (!filialExists)
                    throw new NotFoundException($"Filial {dto.FilialId} não encontrada.");
            }

            entity.Placa = placaNorm;
            entity.Modelo = dto.Modelo;
            entity.Marca = dto.Marca;
            entity.Ano = dto.Ano;
            entity.Status = dto.Status;
            entity.FilialId = dto.FilialId;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;

            await _db.SaveChangesAsync(ct);
            await _db.Entry(entity).Reference(e => e.Filial).LoadAsync(ct);

            return MapToResponse(entity);
        }

        // ===============================
        // DELETE: remover
        // ===============================
        public async Task DeleteAsync(long id, CancellationToken ct = default)
        {
            var entity = await _db.Motos.FirstOrDefaultAsync(m => m.Id == id, ct);
            if (entity is null)
                throw new NotFoundException($"Moto {id} não encontrada.");

            // Verificações de vínculo sem carregar coleções:
            var temAgendamentos = await _db.Agendamentos.AsNoTracking().AnyAsync(a => a.MotoId == id, ct);
            var temEventos = await _db.Eventos.AsNoTracking().AnyAsync(e => e.MotoId == id, ct);

            if (temAgendamentos || temEventos)
                throw new DomainValidationException("Não é possível excluir a moto: existem registros relacionados.");

            _db.Motos.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }


        // ===============================
        // Mapper (Entity -> Response)
        // ===============================
        private static MotoResponse MapToResponse(Moto m) => new()
        {
            Id = m.Id,
            Placa = m.Placa,
            Modelo = m.Modelo,
            Marca = m.Marca,
            Ano = m.Ano,
            Status = m.Status,
            FilialId = m.FilialId,
            Latitude = m.Latitude,
            Longitude = m.Longitude
        };

        // ===============================
        // Helpers
        // ===============================
        /// <summary>
        /// Normaliza a placa: trim, remove hífen e aplica UPPER.
        /// Mantém apenas letras/dígitos (útil para evitar variações ao comparar).
        /// </summary>
        private static string NormalizePlaca(string? placa)
        {
            if (string.IsNullOrWhiteSpace(placa)) return string.Empty;

            var trimmed = placa.Trim().ToUpperInvariant();

            // Remove hífens e espaços internos
            trimmed = trimmed.Replace("-", "").Replace(" ", "");

            // (Opcional) manter só A-Z e 0-9 para evitar caracteres estranhos
            var chars = trimmed.Where(c =>
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9')
            );

            return new string(chars.ToArray());
        }
    }
}
