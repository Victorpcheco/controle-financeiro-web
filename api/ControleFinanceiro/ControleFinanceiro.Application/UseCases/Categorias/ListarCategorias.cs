using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias;

public class ListarCategorias(
    ICategoriaRepository repository,
    IMapper mapper,
    IUserContext userContext
) : IListarCategorias
{
    public async Task<CategoriaPaginadoResponse> ListarCategoriaPaginado(int pagina, int quantidadePorPagina)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidadePorPagina < 1)
            throw new ArgumentException("A quantidade deve ser maior ou igual a 1.");

        var usuarioId = userContext.UsuarioId;

        var categorias = await repository.ListarCategoriaPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        var total = await repository.ContarTotalAsync();
        var categoriasMapper = categorias.Select(x => mapper.Map<CategoriaResponse>(x)).ToList();
            
        return new CategoriaPaginadoResponse()
        {
            Total = total,
            Pagina = pagina,
            Quantidade = quantidadePorPagina,
            Categorias = categoriasMapper,
        };
    }
}