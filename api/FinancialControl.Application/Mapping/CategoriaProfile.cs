using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Mapping;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CategoriaRequestDto, Categoria>();
        CreateMap<Categoria, CategoriaResponseDto>();
    }
}