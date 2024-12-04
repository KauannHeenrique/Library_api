using Microsoft.EntityFrameworkCore;
using Library_api.Models;
using System;

namespace Library_api.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Locatario> Locatarios { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Emprestimo>()
                 .HasOne(rb => rb.Livro)
                 .WithMany()
                 .HasForeignKey(rb => rb.LivroId);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(rb => rb.Locatario)
                .WithMany()
                .HasForeignKey(rb => rb.LocatarioId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
