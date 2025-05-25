using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces;

public interface IContaBancariaRepository
{
    Task<IReadOnlyList<ContaBancaria>> ListarContasPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
    Task<int> ContarTotalAsync();
    Task<ContaBancaria?> BuscarContaPorIdAsync(int id);
    Task CriarContaAsync(ContaBancaria contaBancaria);
    Task AtualizarContaAsync(ContaBancaria contaBancaria);
    Task DeletarContaAsync(ContaBancaria contaBancaria);
}