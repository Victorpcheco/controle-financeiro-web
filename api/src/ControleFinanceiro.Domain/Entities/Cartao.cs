namespace ControleFinanceiro.Domain.Entities;

public class Cartao
{
    public int Id { get; private set; }
    public string NomeCartao { get; private set; } = null!;
    public int UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;
    
    public Cartao() { }
    
    public Cartao(string nome, int usuarioId)
    {
        ValidarNome(nome);
        
        NomeCartao = nome;
        UsuarioId = usuarioId;
    }
    
    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cartão não pode ser vazio ou nulo.");
        if (nome.Length < 3)
            throw new ArgumentException("O nome do cartão deve ter pelo menos 3 caracteres.");
    }
    
    public void AtualizarCartao(string nome)
    {
        ValidarNome(nome);
        NomeCartao = nome;
    }
}