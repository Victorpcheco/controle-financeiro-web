using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Categorias;

public interface ICriarCategoria
{
    Task<bool> CriarNovaCategoria(CategoriaCriarRequest criarRequest);
}