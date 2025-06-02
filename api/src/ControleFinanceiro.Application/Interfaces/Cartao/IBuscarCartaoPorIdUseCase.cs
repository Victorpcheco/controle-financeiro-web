using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Cartao;

public interface IBuscarCartaoPorIdUseCase
{
    Task<CartaoResponseDto> ExecuteAsync(int id);
}