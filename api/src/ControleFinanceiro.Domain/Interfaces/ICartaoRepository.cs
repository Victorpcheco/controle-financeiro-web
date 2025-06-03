using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces;

public interface ICartaoRepository
{
    Task <IReadOnlyList<Cartao>> ListarCartaoPaginadoAsync (int usuarioId, int pagina, int quantidade);
    Task <int> ContarTotalAsync(int usuarioId);
    Task <Cartao?> BuscarCartaoPorId (int id);
    Task AdicionarCartaoAsync(Cartao cartao);
    Task AtualizarCartaoAsync(Cartao cartao);
    Task DeletarCartaoAsync(Cartao cartao);
}