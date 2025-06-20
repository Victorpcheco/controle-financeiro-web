using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal
{
    public class ObterSaldoTotalUseCase(IDashboardRepository repository, IUserContext userContext) : IObterSaldoTotalUseCase
    {

        public async Task<decimal> ExecuteAsync()
        {
            try
            {
                var usuarioId = userContext.UsuarioId;
                return await repository.ObterSaldoTotalAsync(usuarioId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter saldo total", ex);
            }
        }
    }
}
