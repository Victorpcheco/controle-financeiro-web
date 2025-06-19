using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiro.Domain.Entities
{
    [NotMapped]
    public  class SaldoConta
    {
        public int Id { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
        public SaldoConta(string nomeConta, decimal saldo)
        {
            NomeConta = nomeConta;
            SaldoAtual = saldo;

        }
    }
}
