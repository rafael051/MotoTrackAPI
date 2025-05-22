using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Data;
using MotoTrackAPI.Models;

namespace MotoTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiliaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FiliaisController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/filiais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> GetAll()
        {
            return await _context.Filiais.ToListAsync();
        }

        // ✅ GET: api/filiais/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Filial>> GetById(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null) return NotFound();
            return filial;
        }

        // ✅ POST: api/filiais
        [HttpPost]
        public async Task<ActionResult<Filial>> Create(Filial filial)
        {
            _context.Filiais.Add(filial);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = filial.Id }, filial);
        }

        // ✅ PUT: api/filiais/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Filial filial)
        {
            if (id != filial.Id) return BadRequest();
            _context.Entry(filial).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/filiais/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial == null) return NotFound();
            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
