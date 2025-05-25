namespace FinancialControl.Application.DTOs;

public class ContaBancariaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Banco { get; set; } = null!;
    public decimal SaldoInicial { get; set; } 
    public decimal? SaldoAtual { get; set; }
    public int UsuarioId { get; set; }
}