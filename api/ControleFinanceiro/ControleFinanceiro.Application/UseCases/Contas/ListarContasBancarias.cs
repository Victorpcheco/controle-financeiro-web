using AutoMapper;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Contas;

public class ListarContasBancarias(
    IContaBancariaRepository repository,
    IMapper mapper,
    IUserContext usercontext) : IListarContasBancarias
{
    public async Task<ContaPaginadoResponse> ListarContasPaginado(int pagina, int quantidadePorPagina)
    {
        if (pagina < 1)
            throw new ArgumentException("A página deve ser maior ou igual a 1.");
        if (quantidadePorPagina < 1)
            throw new ArgumentException("A quantidade por página deve ser maior ou igual a 1.");
        
        var usuarioId = usercontext.UsuarioId;
        
        var contas = await repository.ListarContasPaginadoAsync(usuarioId, pagina, quantidadePorPagina);
        if (contas == null)
            throw new ArgumentException("Nenhuma conta encontrada.");
        
        var contasDto = contas.Select(x => mapper.Map<ContaResponse>(x)).ToList();
        var total = await repository.ContarTotalAsync();

        return new ContaPaginadoResponse
        {
            Total = total,
            Pagina = pagina,
            Quantidade = quantidadePorPagina,
            Contas = contasDto,
        };
    }
}