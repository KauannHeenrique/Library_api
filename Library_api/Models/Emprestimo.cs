using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_api.Models
{
    public class Emprestimo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int LivroId { get; set; }
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        [Required]
        public int LocatarioId { get; set; }
        [ForeignKey("LocatarioId")]
        public Locatario? Locatario { get; set; }  

        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; } 
    }
}
