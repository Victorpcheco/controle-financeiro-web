namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.DeletarMesReferencia;

public interface IDeletarMesReferenciaUseCase
{
    public Task<bool> ExecuteAsync(int id);
}