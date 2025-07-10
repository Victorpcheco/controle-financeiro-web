using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Contas.ListarContaBancaria;

public class ListarContasBancariasUseCase(
    IContaBancariaRepository repository,
    IMapper mapper,
    IUserContext usercontext) : IListarContasBancariasUseCase
{
    public async Task<ContaBancariaPaginadoResponseDto> ExecuteAsync(PaginadoRequestDto requestDto)
    {
        if (requestDto.Pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (requestDto.Quantidade < 1)
            throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");
        
        var usuarioId = usercontext.UsuarioId;
        
        var contas = await repository.ListarContasPaginadoAsync(usuarioId, requestDto.Pagina, requestDto.Quantidade);
        if (contas == null)
            throw new ArgumentException("Nenhuma conta encontrada.");
        
        var contasDto = contas.Select(x => mapper.Map<ContaBancariaResponseDto>(x)).ToList();
        var total = await repository.ContarTotalAsync(usuarioId);

        return new ContaBancariaPaginadoResponseDto
        {
            Total = total,
            Pagina = requestDto.Pagina,
            Quantidade = requestDto.Quantidade,
            Contas = contasDto,
        };
    }
}