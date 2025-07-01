using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;
using FluentValidation;

namespace ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;

public class AtualizarMovimentacaoUseCase(IMovimentacoesRepository repository, IValidator<MovimentacaoCriarDto> validator)
    : IAtualizarMovimentacaoUseCase
{
    public async Task<bool> ExecuteAsync(int id, MovimentacaoCriarDto request)
    {

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (id <= 0)
            throw new ArgumentException("Id inválido.");

        var movimentacoes = await repository.BuscarMovimentacoesPorIdAsync(id);
        if (movimentacoes == null)
            throw new KeyNotFoundException("Despesa não encontrada.");

        movimentacoes.AtualizarMovimentacao(request.Titulo, request.DataVencimento, request.MesReferenciaId, request.CategoriaId,
            request.CartaoId ?? 0, request.ContaBancariaId, request.Valor, request.Realizado, request.FormaDePagamento ?? string.Empty);

        await repository.AtualizarMovimentacoesAsync(movimentacoes);
        return true;
    }
}
