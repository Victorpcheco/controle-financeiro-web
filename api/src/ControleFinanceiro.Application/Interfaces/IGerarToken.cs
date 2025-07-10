using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Interfaces;

public interface IGerarToken
{
    public string GeraToken(Usuario usuario);
}