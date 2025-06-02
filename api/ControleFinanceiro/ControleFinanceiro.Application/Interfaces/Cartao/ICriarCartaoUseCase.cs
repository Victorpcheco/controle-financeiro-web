using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Cartao;

public interface ICriarCartaoUseCase
{
    Task<bool> ExecuteAsync(CartaoCriarDto dto);
}