using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Application.Dtos;

public class CategoriaCriarDto
{
    public string NomeCategoria { get; set; } = null!;
    public Tipo Tipo { get; set; }
}