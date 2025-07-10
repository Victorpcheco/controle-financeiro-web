using ControleFinanceiro.Application.DTOs;

namespace ControleFinanceiro.Application.Dtos;

public class MovimentacaoPaginadoResponseDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public List<MovimentacaoResponseDto> Movimentacoes { get; set; } = new();
}