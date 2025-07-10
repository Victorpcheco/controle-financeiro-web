using System.ComponentModel.DataAnnotations.Schema;
using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Domain.Entities
{
    [NotMapped]
    public class MovimentacoesResumo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public DateOnly DataVencimento { get; set; }
        public Tipo Tipo { get; set; }
        public decimal Valor { get; set; }
        public bool Realizado { get; set; }
        public int MesReferenciaId { get; set; }
        public int CategoriaId { get; set; }
        public int? CartaoId { get; set; }
        public int ContaBancariaId { get; set; }
        public int UsuarioId { get; set; }
        public string MesReferenciaNome { get; set; } = null!;
        public string CategoriaNome { get; set; } = null!;
        public string ContaBancariaNome { get; set; } = null!;
        public string? CartaoNome { get; set; }
        public string? FormaDePagamento { get; set; } = null!;
    }
}
