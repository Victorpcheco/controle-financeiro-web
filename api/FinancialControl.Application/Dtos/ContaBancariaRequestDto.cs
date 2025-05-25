namespace FinancialControl.Application.DTOs;

public class ContaBancariaRequestDto
{
    public string Nome { get; set; } = null!;
    public string Banco { get; set; } = null!;
    public decimal SaldoInicial { get; set; } 
}