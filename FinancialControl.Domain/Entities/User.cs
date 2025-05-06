using System.Net.Mail;

namespace FinancialControl.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string NomeCompleto { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string SenhaHash { get; private set; } = string.Empty;
        public string ?RefreshToken { get; private set; }
        public DateTime ?RefreshTokenExpiracao { get; private set; }

        public User() { }

        public User(string nomeCompleto, string email, string senhaHash)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            SenhaHash = senhaHash;
        }

        public static User Criar(string nomeCompleto, string email, string senhaHash)
        {
            ValidarNomeCompleto(nomeCompleto);
            ValidarEmail(email);
            ValidarSenha(senhaHash);
            return new User(nomeCompleto, email, senhaHash);
        }

        private static void ValidarNomeCompleto(string nomeCompelto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompelto))
                throw new ArgumentException("Nome completo não pode ser vazio ou nulo.", nameof(nomeCompelto));
            if (nomeCompelto.Length > 100) 
                throw new ArgumentException("Nome completo não pode ter mais de 100 caracteres.", nameof(nomeCompelto));
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio ou nulo.", nameof(email));
            try
            {
                var addr = new MailAddress(email);
            }
            catch
            {
                throw new ArgumentException("Email inválido.", nameof(email));
            }
        }

        private static void ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia ou nula.");

            if (senha.Length < 6)
                throw new ArgumentException("Senha deve ter pelo menos 6 caracteres.");

            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$");
            if (!regex.IsMatch(senha))
                throw new ArgumentException("Senha deve conter pelo menos uma letra maiúscula, um número e um caractere especial.");
        }
    }
}
