using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces;

public interface IContaBancariaService
{
    Task<ContaBancariaPaginadoResponseDto> ListarContasPaginado(int usuarioId, int pagina, int quantidadePorPagina);
}