namespace FinancialControl.Application.DTOs;

public class ContaBancariaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Banco { get; set; }
    public decimal SaldoInicial { get; private set; } 
    public decimal? SaldoAtual { get; private set; }
    public int UsuarioId { get; set; }
}