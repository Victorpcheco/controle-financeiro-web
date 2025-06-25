using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto
{
    public class ListarMovimentacoesEmAbertoUseCase(IDashboardRepository repository, IUserContext userContext) : IListarMovimentacoesEmAbertoUseCase
    {
        public async Task<List<MovimentacoesResumo>> ExecuteAsync()
        {
            var usuarioId = userContext.UsuarioId;
            return await repository.ListarMovimentacoesEmAbertoAsync(usuarioId);
        }

    }
}
