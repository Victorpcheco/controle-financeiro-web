using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enum;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Repositories
{
    public class MovimentacoesRepository(ApplicationDbContext context) : IMovimentacoesRepository
    {

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoAsync(int usuarioId, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesReceitasPaginadoAsync(int usuarioId, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.Tipo == Tipo.Receita)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesDespesasAsync(int usuarioId)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.Tipo == Tipo.Despesa)
                .CountAsync();
        }

        public async Task<int> ContarMovimentacoesReceitasAsync(int usuarioId)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.Tipo == Tipo.Receita)
                .CountAsync();
        }


        public async Task<Movimentacoes?> BuscarMovimentacoesPorIdAsync(int id)
        {
            return await context.Movimentacoes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarMovimentacoesAsync(Movimentacoes Movimentacoes)
        {
            await context.Movimentacoes.AddAsync(Movimentacoes);
            await context.SaveChangesAsync();
        }

        public async Task AtualizarMovimentacoesAsync(Movimentacoes Movimentacoes)
        {
            context.Movimentacoes.Update(Movimentacoes);
            await context.SaveChangesAsync();
        }

        public async Task RemoverMovimentacoesAsync(Movimentacoes Movimentacoes)
        {
            context.Movimentacoes.Remove(Movimentacoes);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.DataVencimento == data)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesPorDataAsync(int usuarioId, DateOnly data)
        {
            return await context.Movimentacoes
                .CountAsync(d => d.UsuarioId == usuarioId && d.DataVencimento == data);
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.ContaBancariaId == contaId)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesPorContaAsync(int usuarioId, int contaId)
        {
            return await context.Movimentacoes
                .CountAsync(d => d.UsuarioId == usuarioId && d.ContaBancariaId == contaId);
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.CategoriaId == categoriaId)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesPorCategoriaAsync(int usuarioId, int categoriaId)
        {
            return await context.Movimentacoes
                .CountAsync(d => d.UsuarioId == usuarioId && d.CategoriaId == categoriaId);
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.Titulo.Contains(titulo))
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesPorTituloAsync(int usuarioId, string titulo)
        {
            return await context.Movimentacoes
                .CountAsync(d => d.UsuarioId == usuarioId && d.Titulo.Contains(titulo));
        }

        public async Task<IReadOnlyList<Movimentacoes>> ListarMovimentacoesPaginadoPorCartaoAsync(int usuarioId, int cartaoId, int pagina, int quantidade)
        {
            return await context.Movimentacoes
                .Where(d => d.UsuarioId == usuarioId && d.CartaoId == cartaoId)
                .OrderByDescending(d => d.DataVencimento)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<int> ContarMovimentacoesPorCartaoAsync(int usuarioId, int cartaoId)
        {
            return await context.Movimentacoes
                .CountAsync(d => d.UsuarioId == usuarioId && d.CartaoId == cartaoId);
        }
    }
}

