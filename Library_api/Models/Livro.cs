using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library_api.Models
{
    public class Livro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TituloLivro { get; set; }
        public string AutorLivro { get; set; }
        public int AnoLancamento { get; set; }
        public int QuantidadeDisponivel { get; set; }
    }
}
