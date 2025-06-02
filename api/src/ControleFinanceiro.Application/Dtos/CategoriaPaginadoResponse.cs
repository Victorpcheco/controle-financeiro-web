namespace ControleFinanceiro.Application.Dtos;

public class CategoriaPaginadoResponse
{
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int Quantidade { get; set; }
    public IReadOnlyList<CategoriaResponse?> Categorias { get; set; } = null!;
}