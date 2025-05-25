using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public TipoCategoria Tipo { get; private set; }
        
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; } = null!;

        public Categoria() {}

        public Categoria(string nome, TipoCategoria tipo, int usuarioId)
        {
            ValidarNome(nome);
            ValidarTipo(tipo);
            ValidarUsuarioId(usuarioId);
            
            Nome = nome;
            Tipo = tipo;
            UsuarioId = usuarioId;
        }
        
        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.", nameof(nome));
            
            if (nome.Length > 100)
                throw new ArgumentException("O nome da categoria é muito grande", nameof(nome));
        }
        
        private static void ValidarTipo(TipoCategoria tipo)
        {
            if (!Enum.IsDefined(typeof(TipoCategoria), tipo))
                throw new ArgumentException("Tipo de categoria inválido.", nameof(tipo));
        }
        
        private static void ValidarUsuarioId(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("ID do usuário inválido.", nameof(usuarioId));
        }
        
        public void AtualizarUsuarioId(int usuarioId)
        {
            ValidarUsuarioId(usuarioId);
            UsuarioId = usuarioId;
        }

        public void AtualizarCategoria(string nome, TipoCategoria tipo)
        {
            ValidarNome(nome);
            ValidarTipo(tipo);

            Nome = nome;
            Tipo = tipo;
        }
    }
}
