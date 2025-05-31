using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Contas;

public interface IObterContaBancaria
{
    Task<ContaResponse> ObterContaPorId(int id);
}