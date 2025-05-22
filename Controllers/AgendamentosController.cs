using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.Models;

namespace MotoTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgendamentosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/agendamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agendamento>>> GetAll()
        {
            return await _context.Agendamentos.ToListAsync();
        }

        // ✅ GET: api/agendamentos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Agendamento>> GetById(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null) return NotFound();
            return agendamento;
        }

        // ✅ POST: api/agendamentos
        [HttpPost]
        public async Task<ActionResult<Agendamento>> Create(Agendamento agendamento)
        {
            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = agendamento.Id }, agendamento);
        }

        // ✅ PUT: api/agendamentos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Agendamento agendamento)
        {
            if (id != agendamento.Id) return BadRequest();
            _context.Entry(agendamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/agendamentos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null) return NotFound();
            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
