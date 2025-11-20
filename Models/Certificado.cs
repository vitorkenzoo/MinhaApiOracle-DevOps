using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    [Table("T_CERTIFICADO")]
    public class Certificado
    {
        [Key]
        [MaxLength(10)]
        [Column("ID_CERTIFICADO")]
        public string IdCertificado { get; set; } = null!;

        [Column("DT_EMISSAO")]
        public DateTime DtEmissao { get; set; }

        [MaxLength(200)]
        [Column("DESCRICAO")]
        public string? Descricao { get; set; }

        [MaxLength(50)]
        [Column("CODIGO_VALIDACAO")]
        public string? CodigoValidacao { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("ID_CURSO")]
        public int IdCurso { get; set; }

        // Navegação
        public Usuario? Usuario { get; set; }
        public Curso? Curso { get; set; }
    }
}

