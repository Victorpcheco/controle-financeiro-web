using System.ComponentModel.DataAnnotations;

namespace FinancialControl.Domain.Entities;

public class ContaBancaria
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Banco { get; private set; } = null!;
    public decimal SaldoInicial { get; private set; } 
    public decimal? SaldoAtual { get; private set; }
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
    
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public ContaBancaria(){ }

    public ContaBancaria(string nome, string banco, decimal saldoInicial, int usuarioId)
    {
        ValidarNome(nome);
        ValidarBanco(banco);
        ValidarUsuarioId(usuarioId);
        
        Nome = nome;
        Banco = banco;
        SaldoInicial = saldoInicial;
        SaldoAtual = saldoInicial;
        DataCriacao = DateTime.Now;
        UsuarioId = usuarioId;
    }
    
    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da conta bancária não pode ser nulo ou vazio.");
    }
    
    private static void ValidarBanco(string banco)
    {
        if (string.IsNullOrWhiteSpace(banco))
            throw new ArgumentException("O nome do banco não pode ser nulo ou vazio.");
    }
    
    private static void ValidarUsuarioId(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new ArgumentException("O ID do usuário é inválido.");
    }
    
    public void AtualizarUsuarioId(int usuarioId)
    {
        ValidarUsuarioId(usuarioId);
        UsuarioId = usuarioId;
    }
    
    public void AtualizarContaBancaria(string nome, string banco, decimal saldoInicial)
    {
        Nome = nome;
        Banco = banco;
        SaldoInicial = saldoInicial;
    }
}

 