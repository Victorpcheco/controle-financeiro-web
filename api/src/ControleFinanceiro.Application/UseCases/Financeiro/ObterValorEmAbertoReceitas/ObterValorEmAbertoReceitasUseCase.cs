using AutoMapper;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Financeiro.ObterReceitasEmAberto
{
    public class ObterValorEmAbertoReceitasUseCase(IFinanceiroRepository repository, IUserContext context, IMapper mapper) : IObterValorEmAbertoReceitasUseCase
    {

        public async Task<decimal> ExecuteAsync()
        {
            var usuarioId = context.UsuarioId;

            var valorTotal = await repository.ObterValorEmAbertoReceitasAsync(usuarioId);
            return valorTotal;
        }
    }
}
