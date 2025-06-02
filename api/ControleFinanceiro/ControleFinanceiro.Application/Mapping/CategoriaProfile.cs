using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.Mapping;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CategoriaCriarRequest, Categoria>();
        CreateMap<Categoria, CategoriaResponse>();
    }
}