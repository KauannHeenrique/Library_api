using Library_api.Data;
using Library_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocatariosController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LocatariosController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("ExibirLocatarios")]
        public async Task<ActionResult<IEnumerable<Locatario>>> GetLocatarios()
        {
            return await _context.Locatarios.ToListAsync();
        }

        [HttpGet("buscarLocatario{id}")]
        public async Task<ActionResult<Locatario>> GetLocatario(int id)
        {
            var locatario = await _context.Locatarios.FindAsync(id);

            if (locatario == null)
            {
                return NotFound();
            }

            return locatario;
        }

        [HttpPost("AdicionarLocatário")]
        public async Task<ActionResult<Locatario>> PostLocatario(Locatario locatario)
        {
            _context.Locatarios.Add(locatario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocatario), new { id = locatario.Id }, locatario);
        }

        [HttpPut("AtualizarLocatário{id}")]
        public async Task<IActionResult> PutLocatario(int id, Locatario locatario)
        {
            if (id != locatario.Id)
            {
                return BadRequest();
            }

            _context.Entry(locatario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ExcluirLocatário{id}")]
        public async Task<IActionResult> DeleteLocatario(int id)
        {
            var locatario = await _context.Locatarios.FindAsync(id);
            if (locatario == null)
            {
                return NotFound();
            }

            _context.Locatarios.Remove(locatario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
