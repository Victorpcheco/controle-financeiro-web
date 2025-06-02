using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Categorias;

public interface IObterCategoria
{
    Task<CategoriaResponse> ObterCategoriaPorId(int id);
}