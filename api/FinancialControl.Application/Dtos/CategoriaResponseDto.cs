using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.DTOs;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public TipoCategoria Tipo { get; set; }
    public int UsuarioId { get; set; }
}