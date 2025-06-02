namespace ControleFinanceiro.Domain.Models;

public class MesReferencia
{
    public int Id { get; private set; }
    public string NomeMes { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}