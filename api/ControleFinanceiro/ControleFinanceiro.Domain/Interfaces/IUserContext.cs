namespace ControleFinanceiro.Domain.Interfaces;

public interface IUserContext
{
        int UsuarioId { get; }
        bool IsAuthenticated { get; }
}