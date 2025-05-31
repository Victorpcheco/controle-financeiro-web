using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.Interfaces.Contas;

public interface ICriarContaBancaria
{
    Task<bool> CriarConta(ContaRequest request);
}