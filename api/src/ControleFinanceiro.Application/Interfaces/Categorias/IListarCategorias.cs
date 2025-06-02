using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Categorias;

public interface IListarCategorias
{
    Task<CategoriaPaginadoResponse> ListarCategoriaPaginado(int pagina, int quantidadePorPagina);
}