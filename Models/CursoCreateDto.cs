using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.Models
{
    /// <summary>
    /// DTO para criação de curso (sem relacionamentos)
    /// </summary>
    public class CursoCreateDto
    {
        [Required(ErrorMessage = "O campo 'nomeCurso' é obrigatório.")]
        [MaxLength(150)]
        public string NomeCurso { get; set; } = null!;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo 'qtHoras' é obrigatório.")]
        public int QtHoras { get; set; }
    }
}

