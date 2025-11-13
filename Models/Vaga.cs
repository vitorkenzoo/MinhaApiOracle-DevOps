using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    public class Vaga
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_VAGA")]
        public int IdVaga { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NOME_VAGA")]
        public string NomeVaga { get; set; } = null!;

        [Column("DESCRICAO_VAGA", TypeName = "nvarchar(max)")]
        public string? DescricaoVaga { get; set; }

        [Column("SALARIO", TypeName = "decimal(10,2)")]
        public decimal? Salario { get; set; }

        [Column("DT_PUBLICACAO")]
        public DateTime? DtPublicacao { get; set; }

        [Required]
        [Column("ID_EMPRESA")]
        public int IdEmpresa { get; set; }

        // Navegação
        public Empresa? Empresa { get; set; }
    }
}
