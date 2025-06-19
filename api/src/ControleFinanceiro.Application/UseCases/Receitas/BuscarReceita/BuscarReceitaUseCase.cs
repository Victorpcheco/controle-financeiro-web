using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.BuscarReceita;

public class BuscarReceitaUseCase(IReceitaRepository repository, IMapper mapper) : IBuscarReceitaUseCase
{
    public async Task<ReceitaResponseDto> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O Id é inválido.", nameof(id));

        var receita = await repository.BuscarReceitaPorIdAsync(id);
        if (receita == null)
            throw new KeyNotFoundException("Receita não encontrada.");

        var receitaResponse = mapper.Map<ReceitaResponseDto>(receita);
        return receitaResponse;
    }
}