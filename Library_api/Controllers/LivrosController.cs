using api_lib.Requests;
using Library_api.Data;
using Library_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LivrosController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("ExibirAcervo")]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros()
        {
            return await _context.Livros.ToListAsync();
        }

        [HttpGet("BuscarLivroPor")]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros([FromQuery] string? nomeLivro, [FromQuery] string? nomeAutor)
        {
            var query = _context.Livros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nomeLivro))
            {
                query = query.Where(l => l.TituloLivro.Contains(nomeLivro));
            }

            if (!string.IsNullOrWhiteSpace(nomeAutor))
            {
                query = query.Where(l => l.AutorLivro.Contains(nomeAutor));
            }

            var livros = await query.ToListAsync();

            if (livros.Count == 0)
            {
                return NotFound("Nenhum livro encontrado.");
            }

            return Ok(livros);
        }

        [HttpPost("AdicionarLivro")]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro)
        {
            try
            {
                _context.Livros.Add(livro);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Livro adicionado com sucesso", livro });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro ao adicionar livro", detalhes = ex.Message });
            }
        }

        [HttpPut("AtualizarLivro/{id}")]
        public async Task<IActionResult> PutLivro(int id, LivroRequest livro)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var livroEx = await _context.Livros.FindAsync(id);
            
            if (livroEx == null)
            {
                return NotFound();
            }

            livroEx.AutorLivro = livro.AutorLivro;
            livroEx.TituloLivro = livro.TituloLivro;
            livroEx.AnoLancamento = livro.AnoLancamento;
            livroEx.QuantidadeDisponivel = livro.QuantidadeDisponivel;

            _context.Livros.Update(livroEx);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ExcluirLivro/{id}")]
        public async Task<IActionResult> DeleteLivro(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
