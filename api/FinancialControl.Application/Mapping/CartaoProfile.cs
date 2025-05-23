using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Mapping;

public class CartaoProfile : Profile
{
    public CartaoProfile()
    {
        CreateMap<Cartao, CartaoRequestDto>();
    }
}