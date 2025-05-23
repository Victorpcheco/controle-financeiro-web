using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces;

public interface ICartaoService
{
    Task <CartaoResponsePaginadoDto> ObterCartoesPaginadoAsync(int usuarioId, int pagina, int quantidade);
    Task<Cartao?> ObterCartaoPorIdAsync(int id);
    Task<bool> CriarCartaoAsync(int usuarioId, CartaoRequestDto cartaoRequest);
    Task<bool> AtualizarCartaoAsync(int id, CartaoRequestDto cartaoRequest);
    // Task<bool> DeletarCartaoAsync(int usuarioId, int id);
}