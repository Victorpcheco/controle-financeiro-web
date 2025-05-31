using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Application.Dtos;

public class CategoriaRequest
{
    public string NomeCategoria { get; set; } = null!;
    public TipoCategoria Tipo { get; set; }
}