namespace ControleFinanceiro.Application.Dtos;

public class CartaoResponsePaginadoDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public List<CartaoResponseDto> Cartoes { get; set; } = new();
}