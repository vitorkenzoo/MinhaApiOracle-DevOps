using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    [Table("T_LOG_AUDITORIA")]
    public class LogAuditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_LOG")]
        public int IdLog { get; set; }

        [MaxLength(50)]
        [Column("NOME_TABELA")]
        public string? NomeTabela { get; set; }

        [MaxLength(1)]
        [Column("DS_OPERACAO")]
        public string? DsOperacao { get; set; }

        [MaxLength(50)]
        [Column("DATA_OPERACAO")]
        public string? DataOperacao { get; set; }

        [MaxLength(100)]
        [Column("NM_USUARIO_DB")]
        public string? NmUsuarioDb { get; set; }
    }
}

