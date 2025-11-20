using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.DTOs
{
    /// <summary>
    /// DTO para criação de vaga (sem objetos de navegação)
    /// </summary>
    public class VagaCreateDto
    {
        [Required(ErrorMessage = "O campo 'nomeVaga' é obrigatório.")]
        [MaxLength(100)]
        public string NomeVaga { get; set; } = null!;

        public string? DescricaoVaga { get; set; }

        public decimal? Salario { get; set; }

        public DateTime? DtPublicacao { get; set; }

        [Required(ErrorMessage = "O campo 'idEmpresa' é obrigatório.")]
        public int IdEmpresa { get; set; }
    }
}

