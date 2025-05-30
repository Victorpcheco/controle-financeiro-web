namespace ControleFinanceiro.Domain.Models;

public class CartaoCredito
{
    public int Id { get; private set; }
    public string NomeCartao { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}