using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Mapping;

public class ContaBancariaProfile : Profile
{
    public ContaBancariaProfile()
    {
        CreateMap<ContaBancaria, ContaBancariaRequestDto>();
    }
    
}