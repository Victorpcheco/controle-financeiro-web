using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Categorias;

public interface IAtualizarCategoria
{
    Task<bool> AtualizarCategoriaExistente(int id, CategoriaRequest request);
}