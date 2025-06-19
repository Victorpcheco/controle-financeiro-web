using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Receitas.ListarReceitaPorTitulo
{
    public class ListarReceitaPorTituloUseCase(
        IReceitaRepository repository,
        IUserContext context,
        IMapper mapper
    ) : IListarReceitaPorTituloUseCase
    {
        public async Task<ReceitaPaginadoResponseDto> ExecuteAsync(string titulo, PaginadoRequestDto request)
        {
            if (request.Pagina <= 0 || request.Quantidade <= 0)
                throw new ArgumentException("A página e a quantidade devem ser maior que zero.");

            var usuarioId = context.UsuarioId;

            var receitas = await repository.ListarReceitasPaginadoPorTituloAsync(usuarioId, titulo, request.Pagina, request.Quantidade);
            var receitasResponse = mapper.Map<List<ReceitaResponseDto>>(receitas);
            var totalReceitas = await repository.ContarReceitasPorTituloAsync(usuarioId, titulo);

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
