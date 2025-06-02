using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Cartao;

public interface IAtualizarCartaoUseCase
{
    Task<bool> ExecuteAsync(int id, CartaoCriarDto dto);
}