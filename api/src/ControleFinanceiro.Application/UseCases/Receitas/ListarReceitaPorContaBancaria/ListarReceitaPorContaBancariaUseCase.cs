using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorContaBancaria;

public class ListarReceitaPorContaBancariaUseCase(
    IReceitaRepository repository,
    IUserContext context,
    IMapper mapper
) : IListarReceitaPorContaBancariaUseCase
{
    public async Task<ReceitaPaginadoResponseDto> ExecuteAsync(int contaId, PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var receitas = await repository.ListarReceitasPaginadoPorContaAsync(usuarioId, contaId, request.Pagina, request.Quantidade);
        var receitasResponse = mapper.Map<List<ReceitaResponseDto>>(receitas);
        var totalReceitas = await repository.ContarReceitasPorContaAsync(usuarioId, contaId);

        return new ReceitaPaginadoResponseDto
        {
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = totalReceitas,
            Receitas = receitasResponse
        };
    }
}