namespace ControleFinanceiro.Domain.Models;

public class Usuario
{
    public int Id { get; private set; }
    public string NomeCompleto { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiracao { get; private set; }
        
    public ICollection<Categoria> Categorias { get; set; } = null!;
    public ICollection<ContaBancaria> ContasBancarias { get; set; } = null!;
    public ICollection<CartaoCredito> Cartoes { get; set; } = null!;
    public ICollection<Despesa> Despesas { get; set; } = null!;
    public ICollection<Receita> Receitas { get; set; } = null!;
    public ICollection<MesReferencia> MesesReferencia { get; set; } = null!;
    public ICollection<PlanejamentoCategoria> PlanejamentosCategorias { get; set; } = null!;

}