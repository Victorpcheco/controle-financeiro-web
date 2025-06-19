using ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ControleFinanceiro.Infrastructure.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString, ApplicationDbContext context)
        {
            services.AddScoped<IFinanceiroRepository>(_ => new FinanceiroRepository(connectionString, context));

            // Use Cases
            services.AddScoped<ObterSaldoTotalUseCase>();
            //services.AddScoped<ObterResumoFinanceiroUseCase>();

            return services;
        }
    }
}
