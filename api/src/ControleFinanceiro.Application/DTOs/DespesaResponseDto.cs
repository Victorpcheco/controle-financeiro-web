using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.DTOs
{
    public class DespesaResponseDto
    {
        public int Id { get; set; }
        public string TituloDespesa { get; private set; } = null!;
        public DateOnly Data { get; private set; }
        public int MesReferenciaId { get; private set; }
        public int CategoriaId { get; private set; }
        public int CartaoId { get; private set; }
        public int ContaBancariaId { get; private set; }
        public decimal Valor { get; private set; }
        public bool Realizado { get; private set; } = false;
        public int UsuarioId { get; private set; }

    }
}
