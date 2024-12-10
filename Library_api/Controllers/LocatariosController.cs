using api_lib.Requests;
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

        [HttpGet("BuscarLocatarioPor")]
        public async Task<ActionResult<IEnumerable<Locatario>>> GetLocatario([FromQuery] string? nomeLocatario)
        {
            var query = _context.Locatarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomeLocatario))
            {
                query = query.Where(l => l.NomeLocatario.Contains(nomeLocatario));
            }

            var locatario = await query.ToListAsync();

            if (locatario.Count == 0)
            {
                return NotFound("Nenhum locatário encontrado.");
            }

            return Ok(locatario);
        }

        [HttpPost("AdicionarLocatario")]
        public async Task<ActionResult<Locatario>> PostLocatario(Locatario locatario)
        {
            _context.Locatarios.Add(locatario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocatario), new { id = locatario.Id }, locatario);
        }


        [HttpPut("AtualizarLocatario/{id}")]
        public async Task<IActionResult> PutLocatario(int id, LocatarioRequest locatario)
        {
            if (id <= 0)  
            {
                return BadRequest("ID inválido.");
            }

            var locatarioEx = await _context.Locatarios.FindAsync(id);

            if (locatarioEx == null)
            {
                return NotFound();
            }

            locatarioEx.NomeLocatario = locatario.NomeLocatario;
            locatarioEx.AnoNascimento = locatario.AnoNascimento;

            _context.Locatarios.Update(locatarioEx);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
