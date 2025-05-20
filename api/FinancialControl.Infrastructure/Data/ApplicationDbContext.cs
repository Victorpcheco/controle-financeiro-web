using FinancialControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Categorias)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
