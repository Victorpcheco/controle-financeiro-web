using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorCategoria
{
    public class ListarReceitaPorCategoriaUseCase(
        IReceitaRepository repository,
        IUserContext context,
        IMapper mapper
    ) : IListarReceitaPorCategoriaUseCase
    {
        public async Task<ReceitaPaginadoResponseDto> ExecuteAsync(int categoriaId, PaginadoRequestDto request)
        {
            if (request.Pagina <= 0 || request.Quantidade <= 0)
                throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

            var usuarioId = context.UsuarioId;

            var receitas = await repository.ListarReceitasPaginadoPorCategoriaAsync(usuarioId, categoriaId, request.Pagina, request.Quantidade);
            var receitasResponse = mapper.Map<List<ReceitaResponseDto>>(receitas);
            var totalReceitas = await repository.ContarReceitasPorCategoriaAsync(usuarioId, categoriaId);

            return new ReceitaPaginadoResponseDto
            {
                Pagina = request.Pagina,
                Quantidade = request.Quantidade,
                Total = totalReceitas,
                Receitas = receitasResponse
            };
        }
    }
}
