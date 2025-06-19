using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiro.Domain.Entities
{
    [NotMapped]
    public class SaldoContasPorUsuario
    {
        public int ContaId { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
    }
}
