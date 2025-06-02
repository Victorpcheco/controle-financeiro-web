using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Domain.Interfaces;

public interface ICartaoRepository
{
    Task <IReadOnlyList<CartaoCredito>> ListarCartaoPaginadoAsync (int usuarioId, int pagina, int quantidade);
    Task <int> ContarTotalAsync(int usuarioId);
    Task <CartaoCredito?> BuscarCartaoPorId (int id);
    Task AdicionarCartaoAsync(CartaoCredito cartao);
    Task AtualizarCartaoAsync(CartaoCredito cartao);
    Task DeletarCartaoAsync(CartaoCredito cartao);
}