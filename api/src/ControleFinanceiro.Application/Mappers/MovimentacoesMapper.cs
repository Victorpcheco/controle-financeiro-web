using AutoMapper;
using ControleFinanceiro.Application.DTOs;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers;

public class MovimentacoesMapper : Profile
{
    public MovimentacoesMapper()
    {
        CreateMap<Movimentacoes, MovimentacaoResponseDto>();
    }
}