using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces;

public interface IMesReferenciaRepository
{
    Task <IReadOnlyList<MesReferencia>> ListarMesReferenciaPaginadoAsync (int usuarioId, int pagina, int quantidade);
    Task <int> ContarTotalAsync(int usuarioId);
    Task <MesReferencia?> BuscarMesReferenciaPorId (int id);
    Task AdicionarMesReferenciaAsync(MesReferencia mes);
    Task AtualizarMesReferenciaAsync(MesReferencia mes);
    Task DeletarMesReferenciaAsync(MesReferencia mes);
}