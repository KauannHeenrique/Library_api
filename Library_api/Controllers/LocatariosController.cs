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
        public async Task<ActionResult<IEnumerable<Locatario>>> GetLocatario([FromQuery] string? nomeLocatario, int? anoNascimento)
        {
            var query = _context.Locatarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomeLocatario))
            {
                query = query.Where(loc => loc.NomeLocatario.Contains(nomeLocatario));
            }

            if (anoNascimento.HasValue)
            {
                query = query.Where(loc => loc.AnoNascimento == anoNascimento.Value);
            }

            var locatarios = await query.ToListAsync();

            if (locatarios.Count == 0)
            {
                return NotFound("Nenhum locatário encontrado.");
            }

            return Ok(locatarios);
        }

        [HttpPost("AdicionarLocatario")]
        public async Task<ActionResult<Livro>> PostLocatario(Locatario locatario)
        {
            try
            {
                if (locatario == null)
                {
                    return BadRequest(new { mensagem = "Por favor, preencha todos os campos" });
                }

                var locatarioFirst = await _context.Locatarios.FirstOrDefaultAsync(loc => loc.NomeLocatario == locatario.NomeLocatario
                && loc.AnoNascimento == locatario.AnoNascimento);

                if (locatarioFirst != null)
                {
                    return BadRequest(new { mensagem = "Locatário com nome e ano de nascimento já existe. Adicione mais detalhes." });
                }


                _context.Locatarios.Add(locatario);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Locatário cadastrado com sucesso!", locatario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao cadastrar locatário!", detalhes = ex.Message });
            }
        }


        [HttpPut("AtualizarLocatario/{id}")]
        public async Task<IActionResult> PutLocatario(int id, [FromBody] LocatarioRequest locatario)
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
