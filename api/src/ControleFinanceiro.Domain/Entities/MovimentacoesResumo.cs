using System.ComponentModel.DataAnnotations.Schema;
using ControleFinanceiro.Domain.Enum;

namespace ControleFinanceiro.Domain.Entities
{
    [NotMapped]
    public class MovimentacoesResumo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateOnly DataVencimento { get; set; }
        public Tipo Tipo { get; set; }
        public decimal Valor { get; set; }
        public bool Realizado { get; set; }

        // IDs das relações
        public int MesReferenciaId { get; set; }
        public int CategoriaId { get; set; }
        public int? CartaoId { get; set; }
        public int ContaBancariaId { get; set; }
        public int UsuarioId { get; set; }

        // Dados específicos (sem objetos completos)
        public string CategoriaNome { get; set; }
        public string ContaBancariaNome { get; set; }
        public string? CartaoNome { get; set; }
    }
}
