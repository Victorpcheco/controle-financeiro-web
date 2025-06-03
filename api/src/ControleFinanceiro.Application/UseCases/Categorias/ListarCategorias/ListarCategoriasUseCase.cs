using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias.ListarCategorias;

public class ListarCategoriasUseCase(
    ICategoriaRepository repository,
    IMapper mapper,
    IUserContext userContext
) : IListarCategoriasUseCase
{
    public async Task<CategoriaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto)
    {
        if (requestDto.Pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (requestDto.Quantidade < 1)
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");

        var usuarioId = userContext.UsuarioId;

        var categorias = await repository.ListarCategoriaPaginadoAsync(usuarioId, requestDto.Pagina, requestDto.Quantidade);
        var total = await repository.ContarTotalAsync(usuarioId);
        var categoriasMapper = categorias.Select(x => mapper.Map<CategoriaResponseDto>(x)).ToList();
            
        return new CategoriaPaginadoResponseDto()
        {
            Total = total,
            Pagina = requestDto.Pagina,
            Quantidade = requestDto.Quantidade,
            Categorias = categoriasMapper,
        };
    }
}