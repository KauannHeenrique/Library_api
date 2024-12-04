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

        // GET: api/Emprestimos/ExibirEmprestimos
        [HttpGet("ExibirEmprestimos")]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetEmprestimos()
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)   // Inclui os dados do livro
                .Include(e => e.Locatario) // Inclui os dados do locatário
                .ToListAsync();
        }

        // GET: api/Emprestimos/buscarpor5
        [HttpGet("buscarpor/{id}")]
        public async Task<ActionResult<Emprestimo>> GetEmprestimo(int id)
        {
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

        // POST: api/Emprestimos/adicionar
        [HttpPost("adicionar")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetEmprestimo()
        {
            return await _context.Emprestimos
                .Include(r => r.Livro)
                .Include(r => r.Locatario)
                .ToListAsync();
        }

        // PUT: api/Emprestimos/devolucao{id}
        [HttpPut("devolucao/{id}")]
        public async Task<IActionResult> PutDevolucao(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            // Atualiza a data de devolução
            emprestimo.DataDevolucao = DateTime.Now;

            // Atualiza a quantidade disponível do livro
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

        // DELETE: api/Emprestimos/excluir5
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
