namespace FinancialControl.Application.DTOs;

public class CartaoResponsePaginadoDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public IReadOnlyList<CartaoResponseDto?> Cartoes { get; set; } = null!;
}