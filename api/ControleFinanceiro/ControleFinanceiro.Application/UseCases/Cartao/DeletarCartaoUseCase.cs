using ControleFinanceiro.Application.Interfaces.Cartao;
using ControleFinanceiro.Domain.Interfaces;

namespace ControleFinanceiro.Application.UseCases.Cartao;

public class DeletarCartaoUseCase(ICartaoRepository repository) 
    : IDeletarCartaoUseCase
{
    public async Task<bool> ExecuteAsync(int id)
    {
        var cartao = await repository.BuscarCartaoPorId(id);
        if (cartao == null)
            throw new ArgumentException("Cartão não encontrado.");
    
        await repository.DeletarCartaoAsync(cartao);
        return true;
    }
}