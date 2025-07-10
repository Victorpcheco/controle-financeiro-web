using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas
{
    public class ListarContasComSaldoTotalUseCase(IDashboardRepository repository, IUserContext userContext) : IListarContasComSaldoTotalUseCase
    {
        public async Task<List<SaldoConta>> ExecuteAsync()
        {
            var usuarioId = userContext.UsuarioId;
            var contasPorUsuario = await repository.ListarContasComSaldoTotalAsync(usuarioId);

            var contas = contasPorUsuario.Select(c => new SaldoConta
            {
                Id = c.ContaId,
                NomeConta = c.NomeConta,
                SaldoAtual = c.SaldoAtual
            }).ToList();

            return contas;
        }
    }
}

