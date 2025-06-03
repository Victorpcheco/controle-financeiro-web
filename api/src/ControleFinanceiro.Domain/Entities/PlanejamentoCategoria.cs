namespace ControleFinanceiro.Domain.Entities;

public class PlanejamentoCategoria
{
    public int Id { get; private set; }
    public int MesReferenciaId { get; private set; }
    public MesReferencia MesReferencia { get; private set; } = null!;
    public int CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public decimal ValorPlanejado { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}