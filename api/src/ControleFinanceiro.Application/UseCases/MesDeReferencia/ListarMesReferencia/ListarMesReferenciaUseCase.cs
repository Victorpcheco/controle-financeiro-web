using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.MesDeReferencia.ListarMesReferencia;

public class ListarMesReferenciaUseCase(IMesReferenciaRepository repository, IUserContext context, IMapper mapper) : IListarMesReferenciaUseCase
{
    public async Task<MesReferenciaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto)
    {
        if (requestDto.Pagina < 1 || requestDto.Quantidade < 1)
            throw new ArgumentException("Página e quantidade da página devem ser maiores que zero.");

        var usuarioId = context.UsuarioId;
        
        var lista = await repository.ListarMesReferenciaPaginadoAsync(usuarioId, requestDto.Pagina, requestDto.Quantidade);
        if (lista == null) 
            throw new ArgumentException("Nenhum mês de referência encontrado.");
        
        var meses = lista.Select(x => mapper.Map<MesReferenciaResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync(usuarioId);

        return new MesReferenciaPaginadoResponseDto
        {
            Total = total,
            Pagina = requestDto.Pagina,
            Quantidade = requestDto.Quantidade,
            Meses = meses,
        };
    }
}