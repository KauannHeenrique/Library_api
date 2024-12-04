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

        [HttpGet("BuscarLivroPor/{id}")]
        public async Task<ActionResult<Livro>> GetLivro(int id)
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

            return livro;
        }

        [HttpPost("AdicionarLivro")]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLivro), new { id = livro.Id }, livro);
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

            return NoContent();
        }
    }
}
