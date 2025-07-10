using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data
{
    public class MovimentacoesConfigurationEnum : IEntityTypeConfiguration<Movimentacoes>
    {
        // Configura o save do tipo de categoria como string no banco de dados
        public void Configure(EntityTypeBuilder<Movimentacoes> builder)
        {
            builder
                .Property(c => c.Tipo)
                .HasConversion<string>();
        }

    }
}
