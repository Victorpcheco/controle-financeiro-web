using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Contas;

public class ObterContaBancaria(IContaBancariaRepository repository,
    IMapper mapper) : IObterContaBancaria
{
    public async Task<ContaResponse> ObterContaPorId(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var buscaConta = await repository.BuscarContaPorIdAsync(id);
        if (buscaConta == null)
            throw new ArgumentException("Conta não encontrada.");
        
        var conta = mapper.Map<ContaResponse>(buscaConta);
        
        return conta;
    }
}