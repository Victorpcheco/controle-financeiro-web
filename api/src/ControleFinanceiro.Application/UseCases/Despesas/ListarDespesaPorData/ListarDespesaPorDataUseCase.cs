using System.Globalization;
using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;

public class ListarDespesaPorDataUseCase(IDespesaRepository repository, IUserContext userContext, IMapper mapper) : IListarDespesaPorDataUseCase
{
    public async Task<DespesaPaginadoResponseDto> ExecuteAsync(DateOnly data, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
        
        var usuarioId = userContext.UsuarioId;
        
        var despesas = await repository.ListarDespesasPaginadoPorDataAsync(usuarioId, data, request.Pagina, request.Quantidade);
        var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
        var totalDespesas = await repository.ContarDespesasPorDataAsync(usuarioId, data);

        return new DespesaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalDespesas,
            Despesas = despesasResponse
        };
    }
    
    
}