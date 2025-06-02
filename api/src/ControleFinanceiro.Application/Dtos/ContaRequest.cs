namespace ControleFinanceiro.Application.Dtos;

public class ContaRequest
{
    public string NomeConta { get; set; } = null!;
    public decimal SaldoInicial { get; set; }
}