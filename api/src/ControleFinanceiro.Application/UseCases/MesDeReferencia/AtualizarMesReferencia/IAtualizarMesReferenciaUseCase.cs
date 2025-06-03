using ControleFinanceiro.Application.Dtos;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.AtualizarMesReferencia;

public interface IAtualizarMesReferenciaUseCase
{
    Task<bool> ExecuteAsync(int id, MesReferenciaCriarDto requestDto);
}