using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterDespesasEmAberto
{
    public class ObterValorEmAbertoDespesasUseCase(IDashboardRepository repository, IUserContext userContext) : IObterValorEmAbertoDespesasUseCase
    {
        public async Task<decimal> ExecuteAsync()
        {
            var usuarioId = userContext.UsuarioId;

            var valorTotal = await repository.ObterValorEmAbertoDespesasAsync(usuarioId);

            return valorTotal;
        }
    }
}
