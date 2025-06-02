namespace ControleFinanceiro.Domain.Models;

public class Receita
{
    public int Id { get; private set; }
    public string TituloReceita { get; private set; } = null!;
    public string Data { get; private set; } = null!;
    public int MesReferenciaId { get; private set; }
    public MesReferencia MesReferencia { get; private set; } = null!;
    public int CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public int ContaBancariaId { get; private set; }
    public ContaBancaria ContaBancaria { get; private set; } = null!;
    public decimal Valor { get; private set; }
    public bool Realizado { get; private set; } = false;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
}