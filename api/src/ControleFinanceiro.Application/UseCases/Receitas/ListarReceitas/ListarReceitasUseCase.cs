using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitas;

public class ListarReceitasUseCase(IReceitaRepository repository, IUserContext context, IMapper mapper) : IListarReceitasUseCase
{
    public async Task<ReceitaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto request)
    {
        if (request.Pagina <= 0 || request.Quantidade <= 0)
            throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

        var usuarioId = context.UsuarioId;

        var receitas = await repository.ListarReceitasPaginadoAsync(usuarioId, request.Pagina, request.Quantidade);
        var total = await repository.ContarReceitasAsync(usuarioId);
        var receitasResponse = mapper.Map<List<ReceitaResponseDto>>(receitas);

        return new ReceitaPaginadoResponseDto
        {
            Receitas = receitasResponse,
            Pagina = request.Pagina,
            Quantidade = request.Quantidade,
            Total = total
        };
    }
}