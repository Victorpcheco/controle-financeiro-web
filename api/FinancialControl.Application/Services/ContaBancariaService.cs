using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services;

public class ContaBancariaService : IContaBancariaService
{
    private readonly IContaBancariaRepository _repository;
    private readonly IValidator<ContaBancariaRequestDto> _validator;
    private readonly IMapper _mapper;

    public ContaBancariaService(IContaBancariaRepository repository, IMapper mapper, IValidator<ContaBancariaRequestDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<ContaBancariaPaginadoResponseDto> ListarContasPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidadePorPagina < 1)
            throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");
        
        var contas = await _repository.ListarPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        if (contas == null)
            throw new ArgumentException("Nenhuma conta encontrada.");
        
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
    
    public async Task<ContaBancaria?> BuscarContaPorIdAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await _repository.BuscarPorIdAsync(id);
        return conta;
    }
    
    public async Task<bool> CriarContaAsync(int usuarioId, ContaBancariaRequestDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (usuarioId < 1)
            throw new ArgumentException("Id inválido.");

        var conta = ContaBancaria.Criar(dto.Nome, dto.Banco, dto.SaldoInicial, usuarioId);
        await _repository.AdicionarAsync(conta);
        
        return true;
    }
    
    public async Task<bool> AtualizarContaAsync(int id, ContaBancariaRequestDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await _repository.BuscarPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        conta.AtualizarContaBancaria(dto.Nome, dto.Banco, dto.SaldoInicial);

        await _repository.AtualizarAsync(conta);
        return true;
    }
    
    public async Task<bool> DeletarContaAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await _repository.BuscarPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        await _repository.DeletarAsync(conta);
        return true;
    }
    
}