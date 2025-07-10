using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Cartoes.CriarCartao;

public interface ICriarCartaoUseCase
{
    Task<bool> ExecuteAsync(CartaoCriarDto dto);
}