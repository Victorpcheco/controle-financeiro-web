using System.Data;
using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories
{
    public class FinanceiroRepository : IFinanceiroRepository
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _context;

        public FinanceiroRepository(string connectionString, ApplicationDbContext context)
        {
            _connectionString = connectionString;
            _context = context;
        }

        public async Task<decimal> ObterSaldoTotalAsync(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@UsuarioId", usuarioId, DbType.Int32);

            var resultado = await connection.QuerySingleOrDefaultAsync<decimal>(
                "SP_CALCULA_SALDO_TOTAL",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }

        public async Task<List<SaldoContasPorUsuario>> ListarContasComSaldoTotalAsync(int usuarioId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@UsuarioId", usuarioId, DbType.Int32);

                var result = await connection.QueryAsync<SaldoContasPorUsuario>(
                    "SP_LISTA_SALDOS_CONTAS",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 30
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar saldos das contas: {ex.Message}", ex);
            }
        }

        public async Task<decimal> ObterValorEmAbertoDespesasAsync(int usuarioId)
        {
            return await _context.Despesas
                .Where(r => r.UsuarioId == usuarioId && r.Realizado == false)
                .SumAsync(r => r.Valor);
        }

        public async Task<decimal> ObterValorEmAbertoReceitasAsync(int usuarioId)
        {
            return await _context.Receitas
                .Where(r => r.UsuarioId == usuarioId && r.Realizado == false)
                .SumAsync(r => r.Valor);
        }
    }
}
