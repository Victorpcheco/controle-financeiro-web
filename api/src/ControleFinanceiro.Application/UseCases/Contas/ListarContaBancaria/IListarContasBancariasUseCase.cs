using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Contas.ListarContaBancaria;

public interface IListarContasBancariasUseCase
{
    Task<ContaBancariaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto);
}