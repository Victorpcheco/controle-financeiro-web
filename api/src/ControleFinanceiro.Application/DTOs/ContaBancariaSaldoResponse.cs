using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.Application.DTOs
{
    public class ContaBancariaSaldoResponse
    {
        public int Id { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
    }
}
