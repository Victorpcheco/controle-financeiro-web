using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.Interfaces.Token;

public interface IGerarToken
{
    public string GeraToken(Usuario usuario);
}