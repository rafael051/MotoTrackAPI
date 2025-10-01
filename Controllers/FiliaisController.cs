// File: Controllers/FiliaisController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;

// usamos DTOs (não expomos entidades nos endpoints)
using MotoTrackAPI.DTO.Filial.Request;
using MotoTrackAPI.DTO.Filial.Response;

// Examples (Swagger)
using MotoTrackAPI.Swagger.Examples.Filial;

namespace MotoTrackAPI.Controllers
{
    /// <summary>
    /// 🏢 Controller: Filiais
    /// ------------------------------------------------------------
    /// - Retorna sempre DTOs (nunca entidades)
    /// - Sem objetos aninhados (ex.: não retornamos a coleção de Motos aqui)
    /// - Datas/formatos são tratados pelo pipeline JSON (Program.cs), não aqui
    /// - Códigos HTTP padronizados: 200/201/204/400/404/409
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Operações de Filiais")]
    public class FiliaisController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FiliaisController(AppDbContext db) => _db = db;

        // =========================================================
        // ✅ GET /api/filiais
        // Lista (sem Include). Mantém paridade com os campos principais.
        // ---------------------------------------------------------
        [HttpGet]
        [SwaggerOperation(Summary = "Listar filiais",
            Description = "Retorna uma lista de filiais com campos principais.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de filiais", typeof(IEnumerable<FilialListItem>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(FilialListItemExample))]
        public async Task<ActionResult<IEnumerable<FilialListItem>>> GetAll(CancellationToken ct)
        {
            var itens = await _db.Filiais
                .AsNoTracking()
                .Select(f => new FilialListItem
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Endereco = f.Endereco,
                    Bairro = f.Bairro,
                    Cidade = f.Cidade,
                    Estado = f.Estado,
                    Cep = f.Cep,
                    Latitude = f.Latitude,
                    Longitude = f.Longitude,
                    RaioGeofenceMetros = f.RaioGeofenceMetros
                })
                .ToListAsync(ct);

            return Ok(itens);
        }

        // =========================================================
        // ✅ GET /api/filiais/{id}
        // Detalhe da filial (sem coleção de motos). Exponho MotoCount.
        // ---------------------------------------------------------
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Obter filial por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Filial encontrada", typeof(FilialResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(FilialResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Filial não encontrada")]
        public async Task<ActionResult<FilialResponse>> GetById(long id, CancellationToken ct)
        {
            var dto = await _db.Filiais
              .AsNoTracking()
              .Where(f => f.Id == id)
              .Select(f => new FilialResponse
              {
                  Id = f.Id,
                  Nome = f.Nome,
                  Endereco = f.Endereco,
                  Bairro = f.Bairro,
                  Cidade = f.Cidade,
                  Estado = f.Estado,
                  Cep = f.Cep,
                  Latitude = f.Latitude,
                  Longitude = f.Longitude,
                  RaioGeofenceMetros = f.RaioGeofenceMetros,
                  // ✅ COUNT numérico (evita TRUE/FALSE)
                  MotoCount = _db.Motos.Where(m => m.FilialId == f.Id).Count()
              })
              .FirstOrDefaultAsync(ct);


            if (dto is null) return NotFound();

            // agrega a contagem de motos (sem retornar a lista)
            var count = await _db.Motos.AsNoTracking().CountAsync(m => m.FilialId == id, ct);
            dto = dto with { MotoCount = count };

            return Ok(dto);
        }

        // =========================================================
        // ✅ POST /api/filiais
        // Criação usando DTO de entrada. Retorna 201 + Location + DTO de saída.
        // ---------------------------------------------------------
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Criar filial")]
        [SwaggerRequestExample(typeof(FilialCreateRequest), typeof(FilialCreateRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Filial criada", typeof(FilialResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(FilialResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao salvar")]
        public async Task<ActionResult<FilialResponse>> Create([FromBody] FilialCreateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = new Models.Filial
            {
                Nome = req.Nome,
                Endereco = req.Endereco,
                Bairro = req.Bairro,
                Cidade = req.Cidade,
                Estado = req.Estado,
                Cep = req.Cep,
                Latitude = req.Latitude,
                Longitude = req.Longitude,
                RaioGeofenceMetros = req.RaioGeofenceMetros
            };

            _db.Filiais.Add(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível salvar a filial. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new FilialResponse
            {
                Id = ent.Id,
                Nome = ent.Nome,
                Endereco = ent.Endereco,
                Bairro = ent.Bairro,
                Cidade = ent.Cidade,
                Estado = ent.Estado,
                Cep = ent.Cep,
                Latitude = ent.Latitude,
                Longitude = ent.Longitude,
                RaioGeofenceMetros = ent.RaioGeofenceMetros,
                MotoCount = 0
            };

            return CreatedAtAction(nameof(GetById), new { id = ent.Id }, resp);
        }

        // =========================================================
        // ✅ PUT /api/filiais/{id}
        // Atualiza apenas campos permitidos (sem trocar PK).
        // ---------------------------------------------------------
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Atualizar filial")]
        [SwaggerRequestExample(typeof(FilialUpdateRequest), typeof(FilialUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Filial não encontrada")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao atualizar")]
        public async Task<IActionResult> Update(long id, [FromBody] FilialUpdateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = await _db.Filiais.FirstOrDefaultAsync(f => f.Id == id, ct);
            if (ent is null) return NotFound();

            // ⬇️ Atualiza somente o contrato permitido
            ent.Nome = req.Nome;
            ent.Endereco = req.Endereco;
            ent.Bairro = req.Bairro;
            ent.Cidade = req.Cidade;
            ent.Estado = req.Estado;
            ent.Cep = req.Cep;
            ent.Latitude = req.Latitude;
            ent.Longitude = req.Longitude;
            ent.RaioGeofenceMetros = req.RaioGeofenceMetros;

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível atualizar a filial. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        // =========================================================
        // ✅ DELETE /api/filiais/{id}
        // Exclusão simples; 404 se não existir.
        // ---------------------------------------------------------
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Excluir filial")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Excluída com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Filial não encontrada")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao excluir")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            var ent = await _db.Filiais.FindAsync(new object[] { id }, ct);
            if (ent is null) return NotFound();

            _db.Filiais.Remove(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível excluir a filial. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
