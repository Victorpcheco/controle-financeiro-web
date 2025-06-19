using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public interface ListarDespesaPorDataUseCase
{
    Task<DespesaPaginadoResponseDto> ExecuteAsync(int usuarioId, string data, int pagina, int quantidade);
}