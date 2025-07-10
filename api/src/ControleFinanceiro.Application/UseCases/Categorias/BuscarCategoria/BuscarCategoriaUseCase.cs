using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Categorias.BuscarCategoria;

public class BuscarCategoriaUseCase(
    ICategoriaRepository repository,
    IMapper mapper) : IBuscarCategoriaUseCase
{
    public async Task<CategoriaResponseDto> ExecuteAsync(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id inválido");

        var buscaCategoria = await repository.BuscarCategoriaPorIdAsync(id); 
        if (buscaCategoria == null)
            throw new KeyNotFoundException("Categoria não encontrada.");
            
        var categoria = mapper.Map<CategoriaResponseDto>(buscaCategoria);

        return categoria;
    }
}