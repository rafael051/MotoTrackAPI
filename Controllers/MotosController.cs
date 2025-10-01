// File: Controllers/MotosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.Data;
// usamos DTOs (não expomos entidades nos endpoints)
using MotoTrackAPI.DTO.Moto.Request;
using MotoTrackAPI.DTO.Moto.Response;
// Examples (Swagger)
using MotoTrackAPI.Swagger.Examples.Moto;

namespace MotoTrackAPI.Controllers
{
    /// <summary>
    /// 🛵 Controller: Motos
    /// ------------------------------------------------------------
    /// - Retorna sempre DTOs (nunca entidades)
    /// - Relações por ID (FilialId opcional), sem objetos aninhados
    /// - Datas/formatos tratados no pipeline JSON (Program.cs), não aqui
    /// - Códigos HTTP padronizados: 200/201/204/400/404/409
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Operações de Motos")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MotosController(AppDbContext db) => _db = db;

        // =========================================================
        // ✅ GET /api/motos
        // Lista enxuta para telas/listas (sem Include).
        // ---------------------------------------------------------
        [HttpGet]
        [SwaggerOperation(Summary = "Listar motos", Description = "Retorna uma lista de motos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de motos", typeof(IEnumerable<MotoListItem>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoListItemExample))]
        public async Task<ActionResult<IEnumerable<MotoListItem>>> GetAll(CancellationToken ct)
        {
            var itens = await _db.Motos
                .AsNoTracking()
                .Select(m => new MotoListItem
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Modelo = m.Modelo,
                    Marca = m.Marca,
                    Ano = m.Ano,
                    Status = m.Status,
                    FilialId = m.FilialId
                })
                .ToListAsync(ct);

            return Ok(itens);
        }

        // =========================================================
        // ✅ GET /api/motos/{id}
        // Detalhe sem navegações; expõe apenas IDs em relações.
        // ---------------------------------------------------------
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Obter moto por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Moto encontrada", typeof(MotoResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        public async Task<ActionResult<MotoResponse>> GetById(long id, CancellationToken ct)
        {
            var dto = await _db.Motos
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MotoResponse
                {
                    Id = m.Id,
                    Placa = m.Placa,
                    Modelo = m.Modelo,
                    Marca = m.Marca,
                    Ano = m.Ano,
                    Status = m.Status,
                    FilialId = m.FilialId,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    DataCriacao = m.DataCriacao
                })
                .FirstOrDefaultAsync(ct);

            return dto is null ? NotFound() : Ok(dto);
        }

        // =========================================================
        // ✅ POST /api/motos
        // Criação usando DTO de entrada (Request).
        // - Valida unicidade da placa
        // - Valida FK de Filial (se enviada)
        // - Retorna 201 + Location + DTO de saída (Response)
        // ---------------------------------------------------------
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Criar moto")]
        [SwaggerRequestExample(typeof(MotoCreateRequest), typeof(MotoCreateRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Moto criada", typeof(MotoResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(MotoResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Filial inexistente (se informada)")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao salvar (ex.: placa duplicada)")]
        public async Task<ActionResult<MotoResponse>> Create([FromBody] MotoCreateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // 🔒 Unicidade de placa (antes)
            // var placaExiste = await _db.Motos.AsNoTracking()
            //     .AnyAsync(m => m.Placa == req.Placa, ct);

            // 🔒 Unicidade de placa (depois)
            var placaExiste = await _db.Motos.AsNoTracking()
                .Where(m => m.Placa == req.Placa)
                .Take(1).CountAsync(ct) > 0;

            if (placaExiste) return Conflict("Já existe uma moto com esta placa.");

            // 🔎 FK Filial (antes)
            // if (req.FilialId is long filialId)
            // {
            //     var filialExiste = await _db.Filiais.AsNoTracking()
            //         .AnyAsync(f => f.Id == filialId, ct);
            //     if (!filialExiste) return NotFound($"Filial {filialId} não encontrada.");
            // }

            // 🔎 FK Filial (depois)
            if (req.FilialId is long filialId)
            {
                var filialExiste = await _db.Filiais.AsNoTracking()
                    .Where(f => f.Id == filialId)
                    .Take(1).CountAsync(ct) > 0;

                if (!filialExiste) return NotFound($"Filial {filialId} não encontrada.");
            }


            var ent = new Models.Moto
            {
                Placa = req.Placa,
                Modelo = req.Modelo,
                Marca = req.Marca,
                Ano = req.Ano,
                Status = req.Status,
                FilialId = req.FilialId,
                Latitude = req.Latitude,
                Longitude = req.Longitude
                // DataCriacao: preferimos DEFAULT no banco; se ainda não houver, você pode setar aqui.
            };

            _db.Motos.Add(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                // cobre violação de unique index em placa ou FK inválida
                return Conflict($"Não foi possível salvar a moto. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            var resp = new MotoResponse
            {
                Id = ent.Id,
                Placa = ent.Placa,
                Modelo = ent.Modelo,
                Marca = ent.Marca,
                Ano = ent.Ano,
                Status = ent.Status,
                FilialId = ent.FilialId,
                Latitude = ent.Latitude,
                Longitude = ent.Longitude,
                DataCriacao = ent.DataCriacao
            };

            return CreatedAtAction(nameof(GetById), new { id = ent.Id }, resp);
        }

        // =========================================================
        // ✅ PUT /api/motos/{id}
        // Atualiza apenas campos permitidos (sem trocar PK).
        // ---------------------------------------------------------
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        [SwaggerOperation(Summary = "Atualizar moto")]
        [SwaggerRequestExample(typeof(MotoUpdateRequest), typeof(MotoUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Atualizada com sucesso")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao atualizar (ex.: placa duplicada)")]
        public async Task<IActionResult> Update(long id, [FromBody] MotoUpdateRequest req, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var ent = await _db.Motos.FirstOrDefaultAsync(m => m.Id == id, ct);
            if (ent is null) return NotFound();

            // 🔒 Unicidade de placa se alterada (substitui o AnyAsync)
            if (!string.Equals(ent.Placa, req.Placa, StringComparison.OrdinalIgnoreCase))
            {
                var placaExiste = await _db.Motos.AsNoTracking()
                    .Where(m => m.Placa == req.Placa && m.Id != id)
                    .Take(1).CountAsync(ct) > 0;

                if (placaExiste) return Conflict("Já existe outra moto com esta placa.");
            }

            // 🔎 Validação de FK (se enviada) (substitui o AnyAsync)
            if (req.FilialId is long filialId)
            {
                var filialExiste = await _db.Filiais.AsNoTracking()
                    .Where(f => f.Id == filialId)
                    .Take(1).CountAsync(ct) > 0;

                if (!filialExiste) return NotFound($"Filial {filialId} não encontrada.");
            }


            // ⬇️ Atualiza somente o contrato permitido
            ent.Placa = req.Placa;
            ent.Modelo = req.Modelo;
            ent.Marca = req.Marca;
            ent.Ano = req.Ano;
            ent.Status = req.Status;
            ent.FilialId = req.FilialId;
            ent.Latitude = req.Latitude;
            ent.Longitude = req.Longitude;
            // ent.DataCriacao permanece imutável

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível atualizar a moto. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }

        // =========================================================
        // ✅ DELETE /api/motos/{id}
        // Exclusão simples; 404 se não existir.
        // ---------------------------------------------------------
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Excluir moto")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Excluída com sucesso")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Moto não encontrada")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflito ao excluir")]
        public async Task<IActionResult> Delete(long id, CancellationToken ct)
        {
            // ajuste: substituir [id] por array para FindAsync
            var ent = await _db.Motos.FindAsync(new object[] { id }, ct);
            if (ent is null) return NotFound();

            _db.Motos.Remove(ent);

            try
            {
                await _db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                return Conflict($"Não foi possível excluir a moto. Detalhes: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent();
        }
    }
}
