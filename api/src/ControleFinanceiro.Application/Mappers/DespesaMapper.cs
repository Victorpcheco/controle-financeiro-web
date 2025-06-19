using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers;

public class DespesaMapper : Profile
{
    public DespesaMapper()
    {
        CreateMap<Despesa, DespesaResponseDto>();
    }
}