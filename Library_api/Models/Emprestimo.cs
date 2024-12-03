namespace Library_api.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        // Chaves estrangeiras
        public int LivroId { get; set; }
        public Livro Livro { get; set; }  // Relação com o Livro

        public int LocatarioId { get; set; }
        public Locatario Locatario { get; set; }  // Relação com o Locatário

        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }  // Nullable para quando não houver devolução
    }
}
