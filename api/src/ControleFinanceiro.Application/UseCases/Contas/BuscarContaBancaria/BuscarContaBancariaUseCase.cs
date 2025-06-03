using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Contas.BuscarContaBancaria;

public class BuscarContaBancariaUseCase(IContaBancariaRepository repository,
    IMapper mapper) : IBuscarContaBancariaUseCase
{
    public async Task<ContaBancariaResponseDto> ExecuteAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var buscaConta = await repository.BuscarContaPorIdAsync(id);
        if (buscaConta == null)
            throw new ArgumentException("Conta não encontrada.");
        
        var conta = mapper.Map<ContaBancariaResponseDto>(buscaConta);
        
        return conta;
    }
}