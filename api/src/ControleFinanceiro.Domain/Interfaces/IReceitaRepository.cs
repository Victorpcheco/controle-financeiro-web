using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces;

public interface IReceitaRepository
{
    Task<IReadOnlyList<Receita>> ListarReceitasPaginadoAsync(int usuarioId, int pagina, int quantidade);
    Task<int> ContarReceitasAsync(int usuarioId);
    Task<Receita?> BuscarReceitaPorIdAsync(int id);
    Task AdicionarReceitaAsync(Receita receita);
    Task AtualizarReceitaAsync(Receita receita);
    Task RemoverReceitaAsync(Receita receita);
    Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorDataAsync(int usuarioId, DateOnly data, int pagina, int quantidade);
    Task<int> ContarReceitasPorDataAsync(int usuarioId, DateOnly data);
    Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorContaAsync(int usuarioId, int contaId, int pagina, int quantidade);
    Task<int> ContarReceitasPorContaAsync(int usuarioId, int contaId);
    Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorCategoriaAsync(int usuarioId, int categoriaId, int pagina, int quantidade);
    Task<int> ContarReceitasPorCategoriaAsync(int usuarioId, int categoriaId);
    Task<IReadOnlyList<Receita>> ListarReceitasPaginadoPorTituloAsync(int usuarioId, string titulo, int pagina, int quantidade);
    Task<int> ContarReceitasPorTituloAsync(int usuarioId, string titulo);
}