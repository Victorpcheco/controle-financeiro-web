using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;

namespace FinancialControl.Application.Services;

public class ContaBancariaService : IContaBancariaService
{
    private readonly IContaBancariaRepository _repository;
    private readonly IMapper _mapper;

    public ContaBancariaService(IContaBancariaRepository repository, IMapper mapper;)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ContaBancariaPaginadoResponseDto> ListarContasPaginado(int usuarioId, int pagina, int quantidadePorPagina)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidadePorPagina < 1)
            throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");
        
        var contas = await _repository.ListarContasPaginado(usuarioId, pagina, quantidadePorPagina);
        var contasDto = contas.Select(x => _mapper.Map<ContaBancariaResponseDto>(x)).ToList();
        var total = await _repository.ContarTotalAsync();

        return new ContaBancariaPaginadoResponseDto
        {
            Total = total,
            Pagina = pagina,
            Quantidade = quantidadePorPagina,
            ContasBancarias = contasDto,
        };

    }
    
}