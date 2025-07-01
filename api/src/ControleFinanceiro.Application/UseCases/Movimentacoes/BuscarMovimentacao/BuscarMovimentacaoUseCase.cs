using AutoMapper;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;

public class BuscarMovimentacaoUseCase(IMovimentacoesRepository repository, IMapper mapper) : IBuscarMovimentacaoUseCase
{
    public async Task<MovimentacaoResponseDto> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O Id é inválido.", nameof(id));

        var movimentacoes = await repository.BuscarMovimentacoesPorIdAsync(id);
        var despesaResponse = mapper.Map<MovimentacaoResponseDto>(movimentacoes);
        if (movimentacoes == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        return despesaResponse;
    }
}