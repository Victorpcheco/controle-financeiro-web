using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.Dtos;

public class CartaoAtualizarDto
{
    public class CartaoAtualizarDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
    }
}