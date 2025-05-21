using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces;

public interface IContaBancariaRepository
{
    Task<IReadOnlyList<ContaBancaria>> ListarContasPaginado(int usuarioId,int pagina, int quantidadePorPagina);
    Task<int> ContarTotalAsync();
    Task<ContaBancaria> BuscarPorId(int id);
    Task AdicionarContaBancaria(ContaBancaria contaBancaria);
    Task AtualizarContaBancaria(ContaBancaria contaBancaria);
    Task DeletarContaBancaria(ContaBancaria contaBancaria);
}