using System.ComponentModel.DataAnnotations;

namespace api_lib.Requests
{
    public class LivroRequest
    {
        [Required]
        public string? TituloLivro { get; set; }

        [Required]
        public string? AutorLivro { get; set; }

        [Required]
        public int AnoLancamento { get; set; }

        [Required]
        public int QuantidadeDisponivel { get; set; }
    }
}

