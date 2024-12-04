using System.ComponentModel.DataAnnotations;

namespace api_lib.Requests
{
    public class LocatarioRequest
    {
        [Required]
        public string? NomeLocatario { get; set; }

        [Required]
        public int AnoNascimento { get; set; }
    }
}

