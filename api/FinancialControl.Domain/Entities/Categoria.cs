using System.ComponentModel.DataAnnotations;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Nome da categoria é muito grande!")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O tipo da categoria é obrigatório!")]
        public TipoCategoria Tipo { get; set; }
    }
}
