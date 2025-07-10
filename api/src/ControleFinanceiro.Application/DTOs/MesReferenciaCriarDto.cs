using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.Dtos;

public class MesReferenciaCriarDto
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O campo Nome deve ter no máximo 50 caracteres.")]
    public string NomeMes { get; set; } = null!;
}