// File: Controllers/AgendamentosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;
using MotoTrackAPI.DTO.Agendamento.Request;
using MotoTrackAPI.DTO.Agendamento.Response;
using MotoTrackAPI.Swagger.Examples.Agendamento;

namespace MotoTrackAPI.Controllers
{
    /// <summary>
    /// 🎯 Controller: Agendamentos
    /// - Retorna sempre DTOs (nunca entidades)
    /// - Relações por ID (ex.: MotoId), sem objetos aninhados
    /// - Datas no padrão dd/MM/yyyy HH:mm:ss (via JsonConverter global)
    /// - Códigos HTTP padronizados: 200/201/204/400/404/409
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Operações de Agendamentos")]
    public class AgendamentosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AgendamentosController(AppDbContext db) => _db = db;

        // =========================================================
        // GET /api/agendamentos
        // Lista enxuta (ideal para telas/listas)
        // =========================================================
        [HttpGet]
        [SwaggerOperation(Summary = "Listar agendamentos",
            Description = "Retorna uma lista simplificada de agendamentos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de agendamentos",
            typeof(IEnumerable<AgendamentoListItem>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AgendamentoListItemExample))]
        public async Task<ActionResult<IEnumerable<AgendamentoListItem>>> GetAll(CancellationToken ct)
        {
            var itens = await _db.Agendamentos
                .AsNoTracking()
                .Select(a => new AgendamentoListItem
                {
                    Id = a.Id,
                    MotoId = a.MotoId,
                    DataAgendada = a.DataAgendada,
                    Descricao = a.Descricao
                })
                .ToListAsync(ct);

            return Ok(itens);
        }

        // =========================================================
        // GET /api/agendamentos/{id}
        // Detalhe (sem navegações)
        // =========================================================
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Obter agendamento por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Agendamento encontrado", typeof(AgendamentoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AgendamentoResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamento não encontrado")]
        public async Task<ActionResult<AgendamentoResponse>> GetById(long id, CancellationToken ct)
        {
            var a = await _db.Agendamentos
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new AgendamentoResponse
                {
                    Id = x.Id,
                    MotoId = x.MotoId,
                    DataAgendada = x.DataAgendada,
                    Descricao = x.Descricao,
                    DataCriacao = x.DataCriacao
                })
                .FirstOrDefaultAsync(ct);

            return a is null ? NotFound() : Ok(a);
        }

        // =========================================================
        // POST /api/agendamentos
        // Criação com validações básicas de FK
        // =========================================================
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Criar agendamento")]
        [SwaggerRequestExample(typeof(AgendamentoCreateRequest), typeof(AgendamentoCreateRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Agendamento criado", typeof(AgendamentoResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(AgendamentoResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "FK inexistente (ex.: Moto)")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao salvar")]
        public async Task<ActionResult<AgendamentoResponse>> Create(
            [FromBody] AgendamentoCreateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // 🔎 Validação de FK: Moto deve existir
            var motoExiste = await _db.Motos.AsNoTracking()
            .Where(m => m.Id == req.MotoId)
            .Take(1).CountAsync(ct) > 0;

            if (!motoExiste) return NotFound($"Moto {req.MotoId} não encontrada.");

            // Mapeamento (não expomos entidade no endpoint)
            var ent = new Models.Agendamento
            {
                MotoId = req.MotoId,
                DataAgendada = req.DataAgendada,
                Descricao = req.Descricao
                // DataCriacao: preferir default do banco (trigger/DEFAULT)
            };

            _db.Agendamentos.Add(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível salvar. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new AgendamentoResponse
            {
                Id = ent.Id,
                MotoId = ent.MotoId,
                DataAgendada = ent.DataAgendada,
                Descricao = ent.Descricao,
                DataCriacao = ent.DataCriacao
            };

            return CreatedAtAction(nameof(GetById), new { id = ent.Id }, resp);
        }

        // =========================================================
        // PUT /api/agendamentos/{id}
        // Atualização controlada pelos campos do DTO
        // =========================================================
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Atualizar agendamento")]
        [SwaggerRequestExample(typeof(AgendamentoUpdateRequest), typeof(AgendamentoUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status200OK, "Agendamento atualizado", typeof(AgendamentoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AgendamentoResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamento não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao atualizar")]
        public async Task<ActionResult<AgendamentoResponse>> Update(
            long id, [FromBody] AgendamentoUpdateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = await _db.Agendamentos.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (ent is null) return NotFound();

            // Apenas campos permitidos pelo contrato
            ent.DataAgendada = req.DataAgendada;
            ent.Descricao = req.Descricao;

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível atualizar. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new AgendamentoResponse
            {
                Id = ent.Id,
                MotoId = ent.MotoId,
                DataAgendada = ent.DataAgendada,
                Descricao = ent.Descricao,
                DataCriacao = ent.DataCriacao
            };

            return Ok(resp);
        }

        // =========================================================
        // DELETE /api/agendamentos/{id}
        // Exclusão simples
        // =========================================================
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Excluir agendamento")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Excluído com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamento não encontrado")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao excluir")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            // ⬇️ ajuste: remover uso de [id] (collection expression) que quebra no C# alvo
            var ent = await _db.Agendamentos.FindAsync(new object[] { id }, ct);
            if (ent is null) return NotFound();

            _db.Agendamentos.Remove(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível excluir. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
