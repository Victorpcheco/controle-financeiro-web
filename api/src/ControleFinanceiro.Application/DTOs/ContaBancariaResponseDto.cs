namespace ControleFinanceiro.Application.Dtos;

public class ContaBancariaResponseDto
{
    public int Id { get; set; }
    public string NomeConta { get; set; } = null!;
    public decimal SaldoInicial { get; set; } 
    public int UsuarioId { get; set; }
}