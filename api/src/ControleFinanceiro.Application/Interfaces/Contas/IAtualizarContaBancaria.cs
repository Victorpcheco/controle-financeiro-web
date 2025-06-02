using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Contas;

public interface IAtualizarContaBancaria
{
    Task<bool> AtualizarConta(int id, ContaAtualizarRequest request);
}