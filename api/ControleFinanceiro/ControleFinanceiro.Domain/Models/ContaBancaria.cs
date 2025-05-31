namespace ControleFinanceiro.Domain.Models;

public class ContaBancaria
{
    public int Id { get; private set; }
    public string NomeConta { get; private set; } = null!;
    public decimal SaldoInicial { get; private set; }
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public ContaBancaria() { }

    public ContaBancaria(string nome, decimal saldoInicial, int usuarioId)
    {
        ValidarNomeDaConta(nome);
        
        NomeConta = nome;
        SaldoInicial = saldoInicial;
        UsuarioId = usuarioId;
    }

    private static void ValidarNomeDaConta(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da conta bancária não pode ser nulo ou vazio.", nameof(nome));
        
        if (nome.Length > 100)
            throw new ArgumentException("O nome da conta bancária é muito grande", nameof(nome));
    }
    
    private static void ValidarUsuarioId(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("Id do usuário atribuído é inválido.", nameof(usuarioId));
    }
    
    public void AtualizarUsuarioId(int usuarioId)
    {
        ValidarUsuarioId(usuarioId);
        UsuarioId = usuarioId;
    }
    
    public void AtualizarContaBancaria(string nome)
    {
        ValidarNomeDaConta(nome);
        NomeConta = nome;
    }
}