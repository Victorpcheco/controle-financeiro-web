using ControleFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ControleFinanceiro.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d));

        modelBuilder.Entity<Despesa>()
            .Property(d => d.Data)
            .HasConversion(dateOnlyConverter);
        
        modelBuilder.Entity<Receita>()
            .Property(d => d.Data)
            .HasConversion(dateOnlyConverter);
        
        // chama a configuração da Categoria
        modelBuilder.ApplyConfiguration(new CategoriaConfigurationEnum());
        base.OnModelCreating(modelBuilder);
        
        // Configurações de relacionamento entre entidades
        modelBuilder.Entity<Categoria>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Categorias)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ContaBancaria>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.ContasBancarias)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Cartao>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Cartoes)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Despesa>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Despesas)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Receita>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Receitas)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<MesReferencia>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.MesesReferencia)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<PlanejamentoCategoria>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.PlanejamentosCategorias)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
                
    }
    
    public DbSet<Usuario> Usuarios { get; set;}
    public DbSet<Categoria> Categorias { get; set;}
    public DbSet<ContaBancaria> ContasBancarias { get; set;}
    public DbSet<Cartao> Cartoes { get; set;}
    public DbSet<MesReferencia> MesesReferencia { get; set; }
    public DbSet<PlanejamentoCategoria> PlanejamentosCategorias { get; set; }
    public DbSet<Despesa> Despesas { get; set;}
    public DbSet<Receita> Receitas { get; set;}
}