using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;

// usamos DTOs (não expomos entidades nos endpoints)
using MotoTrackAPI.DTO.Usuario.Request;
using MotoTrackAPI.DTO.Usuario.Response;

// Examples (Swagger)
using MotoTrackAPI.Swagger.Examples.Usuario;

namespace MotoTrackAPI.Controllers
{
    /// <summary>
    /// 👤 Controller: Usuários
    /// ------------------------------------------------------------
    /// - Retorna sempre DTOs (nunca entidades)
    /// - ⚠ Nunca expõe senha em responses
    /// - Relações por ID (FilialId opcional), sem objetos aninhados
    /// - Códigos HTTP padronizados: 200/201/204/400/404/409
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Operações de Usuários")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsuariosController(AppDbContext db) => _db = db;

        // =========================================================
        // ✅ GET: /api/usuarios
        [HttpGet]
        [SwaggerOperation(Summary = "Listar usuários", Description = "Retorna uma lista de usuários (sem senha).")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de usuários", typeof(IEnumerable<UsuarioListItem>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuarioListItemExample))]
        public async Task<ActionResult<IEnumerable<UsuarioListItem>>> GetAll(CancellationToken ct)
        {
            var lista = await _db.Usuarios
                .AsNoTracking()
                .Select(u => new UsuarioListItem
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Perfil = u.Perfil,
                    FilialId = u.FilialId
                })
                .ToListAsync(ct);

            return Ok(lista);
        }

        // =========================================================
        // ✅ GET: /api/usuarios/{id}
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Obter usuário por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Usuário encontrado", typeof(UsuarioResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuarioResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
        public async Task<ActionResult<UsuarioResponse>> GetById(long id, CancellationToken ct)
        {
            var dto = await _db.Usuarios
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UsuarioResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Perfil = u.Perfil,
                    FilialId = u.FilialId
                })
                .FirstOrDefaultAsync(ct);

            return dto is null ? NotFound() : Ok(dto);
        }

        // =========================================================
        // ✅ POST: /api/usuarios
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Criar usuário")]
        [SwaggerRequestExample(typeof(UsuarioCreateRequest), typeof(UsuarioCreateRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Usuário criado", typeof(UsuarioResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(UsuarioResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Filial inexistente (se informada)")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao salvar (e-mail duplicado)")]
        public async Task<ActionResult<UsuarioResponse>> Create([FromBody] UsuarioCreateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // unicidade de e-mail
            var emailExiste = await _db.Usuarios.AsNoTracking()
                .Where(u => u.Email == req.Email)
                .Take(1).CountAsync(ct) > 0;
            if (emailExiste) return Conflict("Já existe um usuário com esse e-mail.");

            // valida FK de Filial (se enviada)
            if (req.FilialId is long filialId)
            {
                var filialExiste = await _db.Filiais.AsNoTracking()
                    .Where(f => f.Id == filialId)
                    .Take(1).CountAsync(ct) > 0;
                if (!filialExiste) return NotFound($"Filial {filialId} não encontrada.");
            }

            var senhaHash = req.Senha; // TODO: aplicar hash real (ex.: BCrypt)

            var ent = new Models.Usuario
            {
                Nome = req.Nome,
                Email = req.Email,
                Senha = senhaHash,
                Perfil = req.Perfil,
                FilialId = req.FilialId
            };

            _db.Usuarios.Add(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível salvar o usuário. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new UsuarioResponse
            {
                Id = ent.Id,
                Nome = ent.Nome,
                Email = ent.Email,
                Perfil = ent.Perfil,
                FilialId = ent.FilialId
            };

            return CreatedAtAction(nameof(GetById), new { id = ent.Id }, resp);
        }

        // =========================================================
        // ✅ PUT: /api/usuarios/{id}
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Atualizar usuário")]
        [SwaggerRequestExample(typeof(UsuarioUpdateRequest), typeof(UsuarioUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao atualizar (e-mail duplicado)")]
        public async Task<IActionResult> Update(long id, [FromBody] UsuarioUpdateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (ent is null) return NotFound();

            // valida FK de Filial (se enviada)
            if (req.FilialId is long filialId)
            {
                var filialExiste = await _db.Filiais.AsNoTracking()
                    .Where(f => f.Id == filialId)
                    .Take(1).CountAsync(ct) > 0;
                if (!filialExiste) return NotFound($"Filial {filialId} não encontrada.");
            }

            // unicidade de e-mail se alterado
            if (!string.Equals(ent.Email, req.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExiste = await _db.Usuarios.AsNoTracking()
                    .Where(u => u.Email == req.Email && u.Id != id)
                    .Take(1).CountAsync(ct) > 0;
                if (emailExiste) return Conflict("Já existe outro usuário com esse e-mail.");
            }

            // atualiza campos permitidos
            ent.Nome = req.Nome;
            ent.Email = req.Email;
            ent.Perfil = req.Perfil;
            ent.FilialId = req.FilialId;

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível atualizar o usuário. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        // =========================================================
        // ✅ DELETE: /api/usuarios/{id}
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Excluir usuário")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Excluído com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao excluir")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var ent = await _db.Usuarios.FindAsync(new object[] { id }, ct);
            if (ent is null) return NotFound();

            _db.Usuarios.Remove(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível excluir o usuário. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
