using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_EMPRESA")]
        public int IdEmpresa { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("RAZAO_SOCIAL")]
        public string RazaoSocial { get; set; } = null!;

        [Required]
        [MaxLength(14)]
        [Column("CNPJ")]
        public string Cnpj { get; set; } = null!;

        [MaxLength(100)]
        [Column("EMAIL_EMPRESA")]
        public string? EmailEmpresa { get; set; }

        // Navegação
        public ICollection<Vaga>? Vagas { get; set; }
    }
}

