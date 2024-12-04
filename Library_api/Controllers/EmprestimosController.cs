using System.Diagnostics.Eventing.Reader;
using api_lib.Requests;
using Library_api.Data;
using Library_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly LibraryContext _context;

        public EmprestimosController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("ExibirEmprestimos")]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetEmprestimo()
        {
            return await _context.Emprestimos
                .Include(r => r.Livro)
                .Include(r => r.Locatario)
                .ToListAsync();
        }

        [HttpGet("buscarEmprestimoPor/{id}")]
        public async Task<ActionResult<Emprestimo>> GetEmprestimo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Livro)
                .Include(e => e.Locatario)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return emprestimo;
        }

        [HttpPost("AdicionarEmprestimo")]
        public async Task<ActionResult<Emprestimo>> PostEmprestimo(LocacaoRequest emprestimo)
        {
            var livro = await _context.Livros.FindAsync(emprestimo.LivroId);
            var locatario = await _context.Locatarios.FindAsync(emprestimo.LocatarioId);

            if (livro == null)
            {
                return NotFound("Livro não encontrado.");
            }

            if (locatario == null)
            {
                return NotFound("Locatário não encontrado.");
            }

            if (livro.QuantidadeDisponivel <= 0)
            {
                return BadRequest("Não há exemplares disponíveis .");
            }

            livro.QuantidadeDisponivel -= 1;

            var locacaoReq = new Emprestimo
            {
                LivroId = emprestimo.LivroId,
                LocatarioId = emprestimo.LocatarioId,
                DataEmprestimo = DateTime.UtcNow
            };

            _context.Emprestimos.Add(locacaoReq);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmprestimo), new { id = locacaoReq.Id }, locacaoReq);
        }

        [HttpPut("DevolucaoLivro/{id}")]
        public async Task<IActionResult> PutDevolucao(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            emprestimo.DataDevolucao = DateTime.Now;

            var livro = await _context.Livros.FindAsync(emprestimo.LivroId);
            if (livro != null)
            {
                livro.QuantidadeDisponivel++;
            }

            _context.Entry(emprestimo).State = EntityState.Modified;
            _context.Entry(livro).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("excluir/{id}")]
        public async Task<IActionResult> DeleteEmprestimo(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
