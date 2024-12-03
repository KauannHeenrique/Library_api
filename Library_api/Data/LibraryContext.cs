using Microsoft.EntityFrameworkCore;
using Library_api.Models;

namespace Library_api.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Locatario> Locatarios { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
    }
}
