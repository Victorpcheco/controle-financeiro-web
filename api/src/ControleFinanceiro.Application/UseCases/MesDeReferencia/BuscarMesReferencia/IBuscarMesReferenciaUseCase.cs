using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.BuscarMesReferencia;

public interface IBuscarMesReferenciaUseCase
{ 
    Task<MesReferenciaResponseDto?> ExecuteAsync(int id);
}