using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaPaginadoResponseDto> ListarCategoriaPaginadoAsync(int usuarioId,int pagina, int quantidadePorPagina);
        Task<Categoria?> ObterCategoriaPorId(int id);
        Task<bool> CriarCategoriaAsync(int usuarioId, CategoriaRequestDto dto);
        Task<bool> AtualizarCategoriaAsync(int id, CategoriaRequestDto dto);
        Task<bool> ExcluirCategoriaAsync(int id);
    }
}
