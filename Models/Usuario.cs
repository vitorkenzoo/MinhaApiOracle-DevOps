using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiOracle.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NOME")]
        public string Nome { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("EMAIL_USUARIO")]
        public string EmailUsuario { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("SENHA")]
        public string Senha { get; set; } = null!;

        [Column("CADASTRO")]
        public DateTime Cadastro { get; set; }

        [Required]
        [MaxLength(14)]
        [Column("CPF")]
        public string Cpf { get; set; } = null!;

        // Navegação
        public ICollection<Certificado>? Certificados { get; set; }
    }
}

