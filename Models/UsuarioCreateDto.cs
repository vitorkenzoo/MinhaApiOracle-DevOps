using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.Models
{
    /// <summary>
    /// DTO para criação de usuário (sem relacionamentos)
    /// </summary>
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "O campo 'nome' é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O campo 'emailUsuario' é obrigatório.")]
        [MaxLength(100)]
        public string EmailUsuario { get; set; } = null!;

        [Required(ErrorMessage = "O campo 'senha' é obrigatório.")]
        [MaxLength(100)]
        public string Senha { get; set; } = null!;

        [Required(ErrorMessage = "O campo 'cpf' é obrigatório.")]
        [MaxLength(14)]
        public string Cpf { get; set; } = null!;

        public DateTime? Cadastro { get; set; }
    }
}

