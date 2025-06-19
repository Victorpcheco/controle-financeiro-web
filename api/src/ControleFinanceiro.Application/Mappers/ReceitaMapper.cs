using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Mappers
{
    public class ReceitaMapper : Profile
    {
        public ReceitaMapper()
        {
            CreateMap<Receita, ReceitaResponseDto>();
        }
    }
}
