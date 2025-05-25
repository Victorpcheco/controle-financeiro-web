using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces;
using FluentValidation;

namespace FinancialControl.Application.Services;

public class ContaBancariaService(    
    IContaBancariaRepository repository,
    IValidator<ContaBancariaRequestDto> validator,
    IMapper mapper) : IContaBancariaService
{
    public async Task<ContaBancariaPaginadoResponseDto> ListarContasPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidadePorPagina < 1)
            throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");
        
        var contas = await repository.ListarContasPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        if (contas == null)
            throw new ArgumentException("Nenhuma conta encontrada.");
        
        var contasDto = contas.Select(x => mapper.Map<ContaBancariaResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync();

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

        var conta = await repository.BuscarContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");
        
        return conta;
    }
    
    public async Task<bool> CriarContaAsync(int usuarioId, ContaBancariaRequestDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (usuarioId < 1)
            throw new ArgumentException("Id inválido.");
        
        var conta = mapper.Map<ContaBancaria>(dto);
        conta.AtualizarUsuarioId(usuarioId);
        // var conta = ContaBancaria.Criar(dto.Nome, dto.Banco, dto.SaldoInicial, usuarioId);
        await repository.CriarContaAsync(conta);
        
        return true;
    }
    
    public async Task<bool> AtualizarContaAsync(int id, ContaBancariaRequestDto dto)
    {
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await repository.ObterContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        conta.AtualizarContaBancaria(dto.Nome, dto.Banco, dto.SaldoInicial);

        await repository.AtualizarContaAsync(conta);
        return true;
    }
    
    public async Task<bool> DeletarContaAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido.");

        var conta = await repository.ObterContaPorIdAsync(id);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.");

        await repository.DeletarContaAsync(conta);
        return true;
    }
    
}