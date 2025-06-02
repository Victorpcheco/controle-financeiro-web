using ControleFinanceiro.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.Data;

public class CategoriaConfigurationEnum: IEntityTypeConfiguration<Categoria>
{
    // Configura o save do tipo de categoria como string no banco de dados
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder
            .Property(c => c.Tipo)
            .HasConversion<string>(); 
    }
}