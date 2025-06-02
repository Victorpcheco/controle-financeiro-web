using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.Mapping;

public class ContaProfile : Profile
{
    public ContaProfile()
    {
        CreateMap<ContaRequest, ContaBancaria>();
        CreateMap<ContaBancaria, ContaResponse>();
    }
}