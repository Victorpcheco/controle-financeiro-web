using FinancialControl.Domain.Entities;

namespace FinancialControl.Domain.Interfaces;

public interface ICartaoRepository
{
    Task <IReadOnlyList<Cartao>> ListarPaginadoAsync (int usuarioId, int pagina, int quantidade);
    Task <int> ContarAsync(int usuarioId);
    Task <Cartao> BuscarPorIdAsync (int id);
    Task AdicionarAsync(Cartao cartao);
    Task AtualizarAsync(Cartao cartao);
    Task RemoverAsync(Cartao cartao);
}