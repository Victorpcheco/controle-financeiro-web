using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces;

public interface IContaBancariaRepository
{
    Task<IReadOnlyList<ContaBancaria>> ListarPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
    Task<int> ContarTotalAsync();
    Task<ContaBancaria> BuscarPorIdAsync(int id);
    Task AdicionarAsync(ContaBancaria contaBancaria);
    Task AtualizarAsync(ContaBancaria contaBancaria);
    Task DeletarAsync(ContaBancaria contaBancaria);
}