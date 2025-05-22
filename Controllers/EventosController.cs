using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.Models;

namespace MotoTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetAll()
        {
            return await _context.Eventos.ToListAsync();
        }

        // ✅ GET: api/eventos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetById(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();
            return evento;
        }

        // ✅ POST: api/eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> Create(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
        }

        // ✅ PUT: api/eventos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Evento evento)
        {
            if (id != evento.Id) return BadRequest();
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/eventos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
