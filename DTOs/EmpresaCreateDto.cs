using System.ComponentModel.DataAnnotations;

namespace MinhaApiOracle.DTOs
{
    /// <summary>
    /// DTO para criação de empresa (sem relacionamentos)
    /// </summary>
    public class EmpresaCreateDto
    {
        [Required(ErrorMessage = "O campo 'razaoSocial' é obrigatório.")]
        [MaxLength(150)]
        public string RazaoSocial { get; set; } = null!;

        [Required(ErrorMessage = "O campo 'cnpj' é obrigatório.")]
        [MaxLength(14)]
        public string Cnpj { get; set; } = null!;

        [MaxLength(100)]
        public string? EmailEmpresa { get; set; }
    }
}

