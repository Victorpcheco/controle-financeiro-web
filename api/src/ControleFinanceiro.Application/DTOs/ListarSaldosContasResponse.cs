using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleFinanceiro.Application.DTOs
{
    public class ListarSaldosContasResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<SaldoContaDto> Dados { get; set; } = new List<SaldoContaDto>();
    }
}
