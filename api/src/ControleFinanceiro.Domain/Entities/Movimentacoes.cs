using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Domain.Entities
{
    public class Movimentacoes
    {
        public int Id { get; private set; }
        public string Titulo { get; private set; } = null!;
        public DateOnly DataVencimento { get; private set; }
        public Tipo Tipo { get; private set; }
        public int MesReferenciaId { get; private set; }
        public MesReferencia MesReferencia { get; private set; } = null!;
        public int CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = null!;
        public int? CartaoId { get; private set; }
        public Cartao? Cartao { get; private set; }
        public int ContaBancariaId { get; private set; }
        public ContaBancaria ContaBancaria { get; private set; } = null!;
        public decimal Valor { get; private set; }
        public bool Realizado { get; private set; } = false;
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; } = null!;
        public string? FormaDePagamento { get; private set; } = null!;

        public Movimentacoes()
        {

        }

        public Movimentacoes(string titulo, DateOnly dataVencimento, Tipo tipo, int mesReferenciaId, int categoriaId, 
            int? cartaoId, int contaBancariaId, decimal valor, bool realizado, int usuarioId, string formaDePagamento)
        {
            ValidarTituloMovimentacao(titulo);
            ValidarDataVencimento(dataVencimento);

            Titulo = titulo;
            DataVencimento = dataVencimento;
            Tipo = tipo;
            MesReferenciaId = mesReferenciaId;
            CategoriaId = categoriaId;
            CartaoId = cartaoId;
            ContaBancariaId = contaBancariaId;
            Valor = valor;
            Realizado = realizado;
            UsuarioId = usuarioId;
            FormaDePagamento = formaDePagamento;
        }


        private static void ValidarTituloMovimentacao(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título da despesa não pode ser vazio ou nulo.");

            if (titulo.Length > 100)
                throw new ArgumentException("Título da despesa não pode ter mais de 100 caracteres.");
        }

        private void ValidarDataVencimento(DateOnly dataVencimento)
        {
            if (dataVencimento == default)
                throw new ArgumentException("Data não pode ser vazia.");

            if (dataVencimento < DateOnly.FromDateTime(DateTime.Now.AddYears(-100)))
                throw new ArgumentException("Data muito antiga.");
        }

        public void AtualizarMovimentacao(string titulo, DateOnly dataVencimento, int mesReferenciaId, int categoriaId, 
            int cartaoId, int contaBancariaId, decimal valor, bool realizado, string formaDePagamento)
        {
            ValidarTituloMovimentacao(titulo);
            ValidarDataVencimento(dataVencimento);

            Titulo = titulo;
            DataVencimento = dataVencimento;
            Tipo = Tipo;
            MesReferenciaId = mesReferenciaId;
            CategoriaId = categoriaId;
            CartaoId = cartaoId;
            ContaBancariaId = contaBancariaId;
            Valor = valor;
            Realizado = realizado;
            FormaDePagamento = formaDePagamento;
        }
    }
}
