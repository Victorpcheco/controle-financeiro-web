namespace ControleFinanceiro.Application.Dtos;

public class ContaBancariaCriarDto
{
    public string NomeConta { get; set; } = null!;
    public decimal SaldoInicial { get; set; }
}