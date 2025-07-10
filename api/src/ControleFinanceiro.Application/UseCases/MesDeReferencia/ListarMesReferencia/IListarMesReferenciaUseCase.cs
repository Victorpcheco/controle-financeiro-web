using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.ListarMesReferencia;

public interface IListarMesReferenciaUseCase
{
    Task<MesReferenciaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto);
}