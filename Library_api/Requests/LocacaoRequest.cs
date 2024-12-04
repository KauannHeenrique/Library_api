using System.ComponentModel.DataAnnotations;

namespace api_lib.Requests
{
    public class LocacaoRequest
    {
        [Required]
        public int LivroId { get; set; }

        [Required]
        public int LocatarioId { get; set; }
    }
}

