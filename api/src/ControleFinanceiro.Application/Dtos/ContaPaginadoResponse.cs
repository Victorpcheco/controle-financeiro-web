namespace ControleFinanceiro.Application.Dtos;

public class ContaPaginadoResponse
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public IReadOnlyList<ContaResponse?> Contas { get; set; } = null!;
}