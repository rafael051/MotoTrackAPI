// File: Controllers/EventosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;

// usando DTOs (não expomos entidades nos endpoints)
using MotoTrackAPI.DTO.Evento.Request;
using MotoTrackAPI.DTO.Evento.Response;

// Examples (Swagger)
using MotoTrackAPI.Swagger.Examples.Evento;

namespace MotoTrackAPI.Controllers
{
    /// <summary>
    /// 🔄 Controller: Eventos
    /// ------------------------------------------------------------
    /// - Retorna sempre DTOs (nunca entidades)
    /// - Relações por ID (ex.: motoId), sem objetos aninhados
    /// - Datas são serializadas/parseadas no pipeline JSON (dd/MM/yyyy HH:mm:ss se configurado)
    /// - Códigos HTTP padronizados: 200/201/204/400/404/409
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Operações de Eventos")]
    public class EventosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EventosController(AppDbContext db) => _db = db;

        // =========================================================
        // ✅ GET /api/eventos
        // Lista enxuta (sem Include), ideal para telas/listas
        // ---------------------------------------------------------
        [HttpGet]
        [SwaggerOperation(Summary = "Listar eventos",
            Description = "Retorna uma lista simplificada de eventos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de eventos", typeof(IEnumerable<EventoListItem>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventoListItemExample))]
        public async Task<ActionResult<IEnumerable<EventoListItem>>> GetAll(CancellationToken ct)
        {
            var itens = await _db.Eventos
                .AsNoTracking()
                .Select(e => new EventoListItem
                {
                    Id = e.Id,
                    MotoId = e.MotoId,
                    Tipo = e.Tipo,
                    Motivo = e.Motivo,
                    DataHora = e.DataHora
                })
                .ToListAsync(ct);

            return Ok(itens);
        }

        // =========================================================
        // ✅ GET /api/eventos/{id}
        // Detalhe com campos essenciais (sem navegações)
        // ---------------------------------------------------------
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Obter evento por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Evento encontrado", typeof(EventoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventoResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Evento não encontrado")]
        public async Task<ActionResult<EventoResponse>> GetById(long id, CancellationToken ct)
        {
            var ev = await _db.Eventos
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EventoResponse
                {
                    Id = x.Id,
                    MotoId = x.MotoId,
                    Tipo = x.Tipo,
                    Motivo = x.Motivo,
                    DataHora = x.DataHora,
                    Localizacao = x.Localizacao
                })
                .FirstOrDefaultAsync(ct);

            return ev is null ? NotFound() : Ok(ev);
        }

        // =========================================================
        // ✅ POST /api/eventos
        // Criação usando DTO de entrada (Request)
        // - Valida FK por ID
        // - Retorna 201 + Location + DTO de saída (Response)
        // ---------------------------------------------------------
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Criar evento")]
        [SwaggerRequestExample(typeof(EventoCreateRequest), typeof(EventoCreateRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Evento criado", typeof(EventoResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EventoResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "FK inexistente (ex.: Moto)")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao salvar")]
        public async Task<ActionResult<EventoResponse>> Create([FromBody] EventoCreateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // 🔎 Validação de FK (motoId deve existir)
            var motoExiste = await _db.Motos.AsNoTracking()
            .Where(m => m.Id == req.MotoId)
            .Take(1).CountAsync(ct) > 0;

            if (!motoExiste) return NotFound($"Moto {req.MotoId} não encontrada.");

            // Mapeamento manual simples (poderíamos usar AutoMapper)
            var ent = new Models.Evento
            {
                MotoId = req.MotoId,
                Tipo = req.Tipo,
                Motivo = req.Motivo,
                DataHora = req.DataHora,       // se não houver DEFAULT no banco, vem do request
                Localizacao = req.Localizacao
            };

            _db.Eventos.Add(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível salvar o evento. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new EventoResponse
            {
                Id = ent.Id,
                MotoId = ent.MotoId,
                Tipo = ent.Tipo,
                Motivo = ent.Motivo,
                DataHora = ent.DataHora,
                Localizacao = ent.Localizacao
            };
            return CreatedAtAction(nameof(GetById), new { id = ent.Id }, resp);
        }

        // =========================================================
        // ✅ PUT /api/eventos/{id}
        // Atualização restrita aos campos permitidos (sem trocar PK)
        // ---------------------------------------------------------
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Atualizar evento")]
        [SwaggerRequestExample(typeof(EventoUpdateRequest), typeof(EventoUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Atualizado com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Evento não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao atualizar")]
        public async Task<IActionResult> Update(long id, [FromBody] EventoUpdateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id, ct);
            if (ent is null) return NotFound();

            // ⬇️ Atualiza apenas o que é permitido pelo contrato
            ent.Tipo = req.Tipo;
            ent.Motivo = req.Motivo;
            ent.DataHora = req.DataHora;
            ent.Localizacao = req.Localizacao;

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível atualizar o evento. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        // =========================================================
        // ✅ DELETE /api/eventos/{id}
        // Exclusão simples; 404 se não existir
        // ---------------------------------------------------------
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Excluir evento")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Excluído com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Evento não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao excluir")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            // ajuste: evitar [id] (collection expression)
            var ent = await _db.Eventos.FindAsync(new object[] { id }, ct);
            if (ent is null) return NotFound();

            _db.Eventos.Remove(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível excluir o evento. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
