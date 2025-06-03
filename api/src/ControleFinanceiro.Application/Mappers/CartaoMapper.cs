using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers;

public class CartaoMapper : Profile
{
    public CartaoMapper()
    {
        CreateMap<Cartao, CartaoResponseDto>();
    }
}