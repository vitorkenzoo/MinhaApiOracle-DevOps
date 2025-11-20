using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.Models
{
    /// <summary>
    /// DTO para criação de certificado (sem objetos de navegação)
    /// </summary>
    public class CertificadoCreateDto
    {
        [Required(ErrorMessage = "O campo 'idCertificado' é obrigatório.")]
        [MaxLength(10)]
        public string IdCertificado { get; set; } = null!;

        public DateTime? DtEmissao { get; set; }

        [MaxLength(200)]
        public string? Descricao { get; set; }

        [MaxLength(50)]
        public string? CodigoValidacao { get; set; }

        [Required(ErrorMessage = "O campo 'idUsuario' é obrigatório.")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O campo 'idCurso' é obrigatório.")]
        public int IdCurso { get; set; }
    }
}

