
namespace ControleFinanceiro.Application.DTOs
{
    public class SaldoContaDto
    {
        public int Id { get; set; }
        public string NomeConta { get; set; } = null!;
        public decimal SaldoAtual { get; set; }
    }
}
