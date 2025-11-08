using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Usuario.Request;    // ✅ Request
using MotoTrackAPI.DTO.Usuario.Response;   // ✅ Response
using MotoTrackAPI.Models;
using MotoTrackAPI.Services.Exceptions;
using MotoTrackAPI.Services.Interfaces;

namespace MotoTrackAPI.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _db;
        public UsuarioService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<UsuarioResponse>> GetAllAsync()
        {
            var list = await _db.Usuarios
                .AsNoTracking()
                .ToListAsync();

            return list.Select(MapToResponse);
        }

        public async Task<UsuarioResponse> GetByIdAsync(long id)
        {
            var entity = await _db.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (entity is null)
                throw new NotFoundException($"Usuário {id} não encontrado.");

            return MapToResponse(entity);
        }

        public async Task<UsuarioResponse> CreateAsync(UsuarioCreateRequest dto)
        {
            // e-mail único
            var emailDup = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (emailDup)
                throw new ConflictException($"Email já cadastrado: {dto.Email}.");

            // filial opcional
            if (dto.FilialId.HasValue)
            {
                var filialOk = await _db.Filiais.AnyAsync(f => f.Id == dto.FilialId.Value);
                if (!filialOk)
                    throw new NotFoundException($"Filial {dto.FilialId} não encontrada.");
            }

            // 🔐 hash de senha (PBKDF2)
            var senhaHash = GerarHashSenha(dto.Senha);

            var entity = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil,
                FilialId = dto.FilialId,   // opcional
                Senha = senhaHash       // coluna de hash/armazenamento
            };

            _db.Usuarios.Add(entity);
            await _db.SaveChangesAsync();

            return MapToResponse(entity);
        }

        public async Task<UsuarioResponse> UpdateAsync(long id, UsuarioUpdateRequest dto)
        {
            var entity = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entity is null)
                throw new NotFoundException($"Usuário {id} não encontrado.");

            // e-mail único (ignorando o próprio registro)
            var conflito = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
            if (conflito)
                throw new ConflictException($"Email já está em uso: {dto.Email}.");

            // valida filial se enviada (pode ser nula para desvincular)
            if (dto.FilialId.HasValue)
            {
                var filialOk = await _db.Filiais.AnyAsync(f => f.Id == dto.FilialId.Value);
                if (!filialOk)
                    throw new NotFoundException($"Filial {dto.FilialId} não encontrada.");
            }

            entity.Nome = dto.Nome;
            entity.Email = dto.Email;
            entity.Perfil = dto.Perfil;
            entity.FilialId = dto.FilialId; // pode ser null

            await _db.SaveChangesAsync();
            return MapToResponse(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entity is null)
                throw new NotFoundException($"Usuário {id} não encontrado.");

            _db.Usuarios.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private static UsuarioResponse MapToResponse(Usuario u) => new()
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Perfil = u.Perfil,
            FilialId = u.FilialId
            // ⚠ Nunca retorne hash/senha
        };

        // ==============================
        // 🔐 Utilitário de hash de senha
        // ==============================
        private static string GerarHashSenha(string senhaPura)
        {
            if (string.IsNullOrWhiteSpace(senhaPura))
                throw new DomainValidationException("Senha inválida.");

            // salt aleatório de 16 bytes
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // PBKDF2 com HMACSHA256, 100k iterações, 32 bytes
            byte[] hash = KeyDerivation.Pbkdf2(
                password: senhaPura,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32);

            // formata: saltBase64.hashBase64
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }
    }
}
