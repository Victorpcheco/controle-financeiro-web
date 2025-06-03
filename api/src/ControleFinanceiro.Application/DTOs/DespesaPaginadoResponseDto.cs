namespace ControleFinanceiro.Application.Dtos;

public class DespesaPaginadoResponseDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public List<DespesaResponseDto> Despesas { get; set; } = new();
}