using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.Mapping;

public class CartaoMappingProfile : Profile
{
    public CartaoMappingProfile()
    {
        CreateMap<CartaoCredito, CartaoResponseDto>();
    }
}