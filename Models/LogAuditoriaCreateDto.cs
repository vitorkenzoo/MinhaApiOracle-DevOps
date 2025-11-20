using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.Models
{
    /// <summary>
    /// DTO para criação de log de auditoria
    /// </summary>
    public class LogAuditoriaCreateDto
    {
        [MaxLength(50)]
        public string? NomeTabela { get; set; }

        [MaxLength(1)]
        public string? DsOperacao { get; set; }

        [MaxLength(50)]
        public string? DataOperacao { get; set; }

        [MaxLength(100)]
        public string? NmUsuarioDb { get; set; }
    }
}

