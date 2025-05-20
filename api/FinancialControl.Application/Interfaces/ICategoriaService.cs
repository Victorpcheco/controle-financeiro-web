using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaPaginadoResponse> ListarPaginadoAsync(int usuarioId,int pagina, int quantidadePorPagina);
        Task<Categoria?> BuscarPorIdAsync(int id);
        Task<bool> CriarCategoriaAsync(CategoriaRequestDto dto);
        Task<bool> AtualizarCategoriaAsync(int id, CategoriaRequestDto dto);
        Task<bool> DeletarCategoriaAsync(int id);

    }
}
