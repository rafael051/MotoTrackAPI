using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Filial.Request;    // ✅ Request
using MotoTrackAPI.DTO.Filial.Response;   // ✅ Response
using MotoTrackAPI.Models;
using MotoTrackAPI.Services.Exceptions;
using MotoTrackAPI.Services.Interfaces;

namespace MotoTrackAPI.Services.Implementations
{
    public class FilialService : IFilialService
    {
        private readonly AppDbContext _db;
        public FilialService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<FilialResponse>> GetAllAsync()
        {
            var list = await _db.Filiais.AsNoTracking().ToListAsync();
            return list.Select(MapToResponse);
        }

        public async Task<FilialResponse> GetByIdAsync(long id)
        {
            var entity = await _db.Filiais.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            if (entity is null) throw new NotFoundException($"Filial {id} não encontrada.");
            return MapToResponse(entity);
        }

        public async Task<FilialResponse> CreateAsync(FilialCreateRequest dto)
        {
            var nomeDup = await _db.Filiais.AnyAsync(f => f.Nome == dto.Nome);
            if (nomeDup) throw new ConflictException($"Já existe filial com nome {dto.Nome}.");

            var entity = new Filial
            {
                Nome = dto.Nome,
                Endereco = dto.Endereco
            };

            _db.Filiais.Add(entity);
            await _db.SaveChangesAsync();
            return MapToResponse(entity);
        }

        public async Task<FilialResponse> UpdateAsync(long id, FilialUpdateRequest dto)
        {
            var entity = await _db.Filiais.FirstOrDefaultAsync(f => f.Id == id);
            if (entity is null) throw new NotFoundException($"Filial {id} não encontrada.");

            var conflito = await _db.Filiais.AnyAsync(f => f.Nome == dto.Nome && f.Id != id);
            if (conflito) throw new ConflictException($"Nome de filial já em uso: {dto.Nome}.");

            entity.Nome = dto.Nome;
            entity.Endereco = dto.Endereco;

            await _db.SaveChangesAsync();
            return MapToResponse(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _db.Filiais
                .Include(f => f.Motos)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (entity is null) throw new NotFoundException($"Filial {id} não encontrada.");

            if (entity.Motos != null && entity.Motos.Any())
                throw new DomainValidationException("Não é possível excluir filial com motos associadas.");

            _db.Filiais.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private static FilialResponse MapToResponse(Filial f) => new()
        {
            Id = f.Id,
            Nome = f.Nome,
            Endereco = f.Endereco
        };
    }
}
