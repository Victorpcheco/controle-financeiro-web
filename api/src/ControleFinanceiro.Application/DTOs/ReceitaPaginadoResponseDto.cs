namespace ControleFinanceiro.Application.Dtos;

public class ReceitaPaginadoResponseDto
{
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public int Total { get; set; }
    public List<ReceitaResponseDto> Receitas { get; set; } = [];

}