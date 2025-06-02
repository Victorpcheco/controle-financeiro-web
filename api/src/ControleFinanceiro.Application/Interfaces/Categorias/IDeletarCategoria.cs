namespace ControleFinanceiro.Application.Interfaces.Categorias;

public interface IDeletarCategoria
{
    Task<bool> DeletarCategoriaExistente(int id);
}