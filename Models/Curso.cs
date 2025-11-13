using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    public class Curso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_CURSO")]
        public int IdCurso { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("NOME_CURSO")]
        public string NomeCurso { get; set; } = null!;

        [Column("DESCRICAO", TypeName = "nvarchar(max)")]
        public string? Descricao { get; set; }

        [Column("QT_HORAS")]
        public int QtHoras { get; set; }

        // Navegação
        public ICollection<Certificado>? Certificados { get; set; }
    }
}

