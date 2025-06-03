using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers;

public class MesReferenciaMapper : Profile
{
    public MesReferenciaMapper()
    {
        CreateMap<MesReferencia, MesReferenciaResponseDto>();
    }
}