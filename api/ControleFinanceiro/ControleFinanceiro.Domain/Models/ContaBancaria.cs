namespace ControleFinanceiro.Domain.Models;

public class ContaBancaria
{
    public int Id { get; private set; }
    public string NomeConta { get; private set; } = null!;
    public decimal SaldoInicial { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}