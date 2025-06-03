using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers;

public class ContaBancariaMapper : Profile
{
    public ContaBancariaMapper()
    {
        CreateMap<ContaBancariaCriarDto, ContaBancaria>();
        CreateMap<ContaBancaria, ContaBancariaResponseDto>();
    }
}