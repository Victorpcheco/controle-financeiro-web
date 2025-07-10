using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Cartoes.BuscarCartao;

public interface IBuscarCartaoUseCase
{
    Task<CartaoResponseDto> ExecuteAsync(int id);
}