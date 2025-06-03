using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.CriarMesReferencia;

public interface ICriarMesReferenciaUseCase
{
    Task<bool> ExecuteAsync(MesReferenciaCriarDto requetDto);
}