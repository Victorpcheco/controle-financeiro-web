using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;

public class ListarMovimentacoesPorContaBancariaUseCase(IMovimentacoesRepository repository,
         IUserContext context, 
         IMapper mapper
         ) : IListarMovimentacoesPorContaBancariaUseCase
     {
         public async Task<MovimentacaoPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request)
         {
             if (request.Pagina <= 0 || request.Quantidade <= 0)
                 throw new ArgumentException("A página e a quantidade devem ser maior que zero.");
             
             var usuarioId = context.UsuarioId;
             
             var movimentacoes = await repository.ListarMovimentacoesPaginadoPorContaAsync(usuarioId, contaId, request.Pagina, request.Quantidade);
             var movimentacoesResponse = mapper.Map<List<MovimentacaoResponseDto>>(movimentacoes);
             var total = await repository.ContarMovimentacoesPorContaAsync(usuarioId, contaId);
     
             return new MovimentacaoPaginadoResponseDto
             {
                 Pagina = request.Pagina,
                 Quantidade = request.Quantidade,
                 Total = total,
                 Movimentacoes = movimentacoesResponse
             };
         }


}