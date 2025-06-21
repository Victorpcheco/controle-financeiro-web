using System.Data;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enum;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories
{
    public class DashboardRepository(ApplicationDbContext context) : IDashboardRepository
    {
        public async Task<decimal> ObterSaldoTotalAsync(int usuarioId)
        {
            var saldoInicial = await context.ContasBancarias
                .Where(c => c.UsuarioId == usuarioId)
                .SumAsync(c => c.SaldoInicial);

            var receitas = await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado == true && m.Tipo == Tipo.Receita)
                .SumAsync(m => m.Valor);

            var despesas = await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado == true && m.Tipo == Tipo.Despesa)
                .SumAsync(m => m.Valor);

            var saldoTotal = saldoInicial + receitas - despesas;

            return saldoTotal;
        }

        public async Task<List<SaldoContasPorUsuario>> ListarContasComSaldoTotalAsync(int usuarioId)
        {
            var contas = await context.ContasBancarias
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new
                {
                    c.Id,
                    c.NomeConta,
                    c.SaldoInicial
                })
                .ToListAsync();

            var movimentacoesPorConta = await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado)
                .GroupBy(m => m.ContaBancariaId)
                .Select(g => new
                {
                    ContaBancariaId = g.Key,
                    TotalReceitas = g.Where(x => x.Tipo == Tipo.Receita).Sum(x => x.Valor),
                    TotalDespesas = g.Where(x => x.Tipo == Tipo.Despesa).Sum(x => x.Valor)
                })
                .ToListAsync();

            var resultado = contas.Select(conta =>
            {
                var movimentacoes = movimentacoesPorConta.FirstOrDefault(m => m.ContaBancariaId == conta.Id);

                decimal receitas = movimentacoes?.TotalReceitas ?? 0;
                decimal despesas = movimentacoes?.TotalDespesas ?? 0;

                return new SaldoContasPorUsuario
                {
                    ContaId = conta.Id,
                    NomeConta = conta.NomeConta,
                    SaldoAtual = conta.SaldoInicial + receitas - despesas
                };
            }).ToList();

            return resultado;

        }

        public async Task<decimal> ObterValorEmAbertoDespesasAsync(int usuarioId)
        {
            return await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado == false && m.Tipo == Tipo.Despesa)
                .SumAsync(m => m.Valor);
        }

        public async Task<decimal> ObterValorEmAbertoReceitasAsync(int usuarioId)
        {
            return await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado == false && m.Tipo == Tipo.Receita)
                .SumAsync(m => m.Valor);
        }

        public async Task<List<Movimentacoes>> ListarMovimentacoesEmAbertoAsync(int usuarioId)
        {
            return await context.Movimentacoes
                .Where(m => m.UsuarioId == usuarioId && m.Realizado == false)
                .OrderByDescending(m => m.DataVencimento)
                .ToListAsync();
        }
    }
}
