
using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Mapping
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioRegistroDto, Usuario>();
            CreateMap<UsuarioLoginDto, Usuario>();
        }
    }
}
