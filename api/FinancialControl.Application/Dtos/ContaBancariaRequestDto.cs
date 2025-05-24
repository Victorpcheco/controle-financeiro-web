namespace FinancialControl.Application.DTOs;

public class ContaBancariaRequestDto
{
    public string Nome { get; set; }
    public string Banco { get; set; }
    public decimal SaldoInicial { get; set; } 
}