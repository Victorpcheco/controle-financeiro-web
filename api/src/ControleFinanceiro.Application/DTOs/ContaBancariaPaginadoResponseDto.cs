namespace ControleFinanceiro.Application.Dtos;

public class ContaBancariaPaginadoResponseDto
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public IReadOnlyList<ContaBancariaResponseDto?> Contas { get; set; } = null!;
}