
using AutoMapper;
using FinancialControl.Application.DTOs;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Mapping
{
    public class UsuarioProfike : Profile
    {
        public UsuarioProfike()
        {
            CreateMap<UsuarioRegistroDto, Usuario>();
            CreateMap<UsuarioLoginDto, Usuario>();
        }
    }
}
