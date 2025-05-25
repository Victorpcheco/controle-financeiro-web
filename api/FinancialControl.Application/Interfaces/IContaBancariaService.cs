using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces;

public interface IContaBancariaService
{
    Task<ContaBancariaPaginadoResponseDto> ListarContasPaginadoAsync(int usuarioId, int pagina, int quantidadePorPagina);
    Task<ContaBancaria?> ObterContaPorIdAsync(int id);
    Task<bool> CriarContaAsync(int usuarioId, ContaBancariaRequestDto dto);
    Task<bool> AtualizarContaAsync(int id, ContaBancariaRequestDto dto);
    Task<bool> DeletarContaAsync(int id);
}