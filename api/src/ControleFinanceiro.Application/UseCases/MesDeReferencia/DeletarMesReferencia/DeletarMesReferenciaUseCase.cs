using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.DeletarMesReferencia;

public class DeletarMesReferenciaUseCase(IMesReferenciaRepository repository) : IDeletarMesReferenciaUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id inválido.");
        
        var mesReferencia = await repository.BuscarMesReferenciaPorId(id);
        if (mesReferencia == null)
            throw new ArgumentException("Mês de referência não encontrado.");
        
        await repository.DeletarMesReferenciaAsync(mesReferencia);
        return true;
    }
}