namespace ControleFinanceiro.Domain.Entities;

public class MesReferencia
{
    public int Id { get; private set; }
    public string NomeMes { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public MesReferencia()
    {
        
    }

    public MesReferencia(string nomeMes, int usuarioId)
    {
        ValidarNomeMes(nomeMes);
        NomeMes = nomeMes;
        UsuarioId = usuarioId;
    }
    
    private static void ValidarNomeMes(string nomeMes)
    {
        if (string.IsNullOrWhiteSpace(nomeMes))
            throw new ArgumentException("O nome do mês não pode ser vazio ou nulo.", nameof(nomeMes));
        if (nomeMes.Length > 50)
            throw new ArgumentException("O nome do mês não pode ter mais de 50 caracteres.", nameof(nomeMes));
    }
    
    public void AtualizarMes(string nomeMes)
    {
        ValidarNomeMes(nomeMes);
        NomeMes = nomeMes;
    }
}