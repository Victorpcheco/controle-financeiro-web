using ControleFinanceiro.Domain.Models;

namespace ControleFinanceiro.Application.Interfaces;

public interface IGerarTokenUseCase
{
    public string GerarToken(Usuario usuario);
}