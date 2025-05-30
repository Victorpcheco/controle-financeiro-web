using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Domain.Models;

public class Categoria
{
    public int Id { get; private set; }
    public string NomeCategoria { get; private set; } = null!;
    public TipoCategoria Tipo { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}