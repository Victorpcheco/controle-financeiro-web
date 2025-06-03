using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Application.Dtos;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string NomeCategoria { get; set; } = null!;
    public TipoCategoria Tipo { get; set; }
    public int UsuarioId { get; set; }
}