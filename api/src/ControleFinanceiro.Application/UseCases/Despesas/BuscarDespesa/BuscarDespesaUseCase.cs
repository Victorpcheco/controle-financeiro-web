using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;

public class BuscarDespesaUseCase(IDespesaRepository repository, IMapper mapper) : IBuscarDespesaUseCase
{
    public async Task<DespesaResponseDto> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("O Id é inválido.", nameof(id));

        var despesa = await repository.BuscarDespesaPorIdAsync(id);
        var despesaResponse = mapper.Map<DespesaResponseDto>(despesa);
        if (despesa == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        return despesaResponse;
    }
}