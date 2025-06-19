using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;

public class ListarDespesaPorContaBancariaUseCase(IDespesaRepository repository,
         IUserContext context, 
         IMapper mapper
         ) : IListarDespesaPorContaBancariaUseCase
     {
         public async Task<DespesaPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request)
         {
             if (request.Pagina <= 0 || request.Quantidade <= 0)
                 throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
             
             var usuarioId = context.UsuarioId;
             
             var despesas = await repository.ListarDespesasPaginadoPorContaAsync(usuarioId, contaId, request.Pagina, request.Quantidade);
             var despesasResponse = mapper.Map<List<DespesaResponseDto>>(despesas);
             var totalDespesas = await repository.ContarDespesasPorContaAsync(usuarioId, contaId);
     
             return new DespesaPaginadoResponseDto
             {
                 Pagina = request.Pagina,
                 Quantidade = request.Quantidade,
                 Total = totalDespesas,
                 Despesas = despesasResponse
             };
         }


}