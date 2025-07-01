using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;

public class CriarMovimentacaoUseCase(IMovimentacoesRepository repository, IUserContext context, IValidator<MovimentacaoCriarDto> validator) : ICriarMovimentacaoUseCase
{
    public async Task<bool> ExecuteAsync(MovimentacaoCriarDto request)
    {
        
        var validationResult = validator.ValidateAsync(request);
        if (!validationResult.Result.IsValid)
            throw new ValidationException(validationResult.Result.Errors);
        
        var usuarioId = context.UsuarioId;

        var movimentacao = new Movimentacoes(request.Titulo, request.DataVencimento, request.Tipo, request.MesReferenciaId, request.CategoriaId,
            request.CartaoId ?? 0, request.ContaBancariaId, request.Valor, request.Realizado, usuarioId, request.FormaDePagamento ?? string.Empty);

        await repository.AdicionarMovimentacoesAsync(movimentacao);
        return true;
    }
}
