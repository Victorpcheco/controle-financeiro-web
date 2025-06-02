using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.Dtos;

public class CartaoCriarDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string NomeCartao { get; set; } = string.Empty;
}