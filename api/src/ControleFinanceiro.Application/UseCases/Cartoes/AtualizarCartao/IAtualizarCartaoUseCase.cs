using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.Cartoes.AtualizarCartao;

public interface IAtualizarCartaoUseCase
{
    Task<bool> ExecuteAsync(int id, CartaoCriarDto dto);
}