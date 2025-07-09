using System.Text;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Application.Mappers;
using ControleFinanceiro.Application.UseCases.Cartoes.AtualizarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.BuscarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.CriarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.DeletarCartao;
using ControleFinanceiro.Application.UseCases.Cartoes.ListarCartao;
using ControleFinanceiro.Application.UseCases.Categorias.AtualizarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.BuscarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.CriarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.DeletarCategoria;
using ControleFinanceiro.Application.UseCases.Categorias.ListarCategorias;
using ControleFinanceiro.Application.UseCases.Contas.AtualizarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.BuscarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.CriarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.DeletarContaBancaria;
using ControleFinanceiro.Application.UseCases.Contas.ListarContaBancaria;
using ControleFinanceiro.Application.UseCases.Dashboard.ListarMovimentacoesEmAberto;
using ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCartao;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorCategoria;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorContaBancaria;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorData;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesaPorTitulo;
using ControleFinanceiro.Application.UseCases.Despesas.ListarMovimentacoesReceitas;
using ControleFinanceiro.Application.UseCases.Financeiro.ListarSaldosContas;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterDespesasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterReceitasEmAberto;
using ControleFinanceiro.Application.UseCases.Financeiro.ObterSaldoTotal;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.AtualizarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.BuscarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.CriarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.DeletarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.ListarMesReferencia;
using ControleFinanceiro.Application.UseCases.Movimentacoess.ListarMovimentacoesPorCategoria;
using ControleFinanceiro.Application.UseCases.Usuarios.LoginUsuario;
using ControleFinanceiro.Application.UseCases.Usuarios.RegistroUsuario;
using ControleFinanceiro.Application.Validators;
using ControleFinanceiro.Domain.Interfaces;
using ControleFinanceiro.Infrastructure.Authentication;
using ControleFinanceiro.Infrastructure.Authentication.Token;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configura o Entity Framework com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra serviços relacionados ao gerenciamento de usuários
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILoginUsuarioUseCase, LoginUsuarioUseCase>();
builder.Services.AddScoped<IRegistroUsuarioUseCase, RegistroUsuarioUseCase>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
builder.Services.AddScoped<IGerarToken, GerarToken>();
builder.Services.AddScoped<IGerarRefreshToken, GerarRefreshToken>();

// Registra todos os use cases e validadores para contas bancárias
builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
builder.Services.AddScoped<IListarContasBancariasUseCase, ListarContasBancariasUseCase>();
builder.Services.AddScoped<IBuscarContaBancariaUseCase, BuscarContaBancariaUseCase>();
builder.Services.AddScoped<ICriarContaBancariaUseCase, CriarContaBancariaUseCase>();
builder.Services.AddScoped<IAtualizarContaBancariaUseCase, AtualizarContaBancariaUseCaseUseCase>();
builder.Services.AddScoped<IDeletarContaBancariaUseCase, DeletarContaBancariaUseCase>();
builder.Services.AddScoped<IValidator<ContaBancariaCriarDto>, ContaBancariaCriarDtoValidator>();
builder.Services.AddScoped<IValidator<ContaBancariaAtualizarDto>, ContaBancariaAtualizarDtoValidator>();

// Registra serviços para gerenciamento de categorias de receitas/despesas
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IListarCategoriasUseCase, ListarCategoriasUseCase>();
builder.Services.AddScoped<IBuscarCategoriaUseCase, BuscarCategoriaUseCase>();
builder.Services.AddScoped<ICriarCategoriaUseCase, CriarCategoriaUseCase>();
builder.Services.AddScoped<IAtualizarCategoriaUseCase, AtualizarCategoriaUseCase>();
builder.Services.AddScoped<IDeletarCategoriaUseCase, DeletarCategoriaUseCase>();
builder.Services.AddScoped<IValidator<CategoriaCriarDto>, CategoriaRequestValidator>();

// Registra serviços para gerenciamento de cartões de crédito/débito
builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
builder.Services.AddScoped<IListarCartaoPaginadoUseCase, ListarCartaoPaginadoUseCase>();
builder.Services.AddScoped<IBuscarCartaoUseCase, BuscarCartaoUseCase>();
builder.Services.AddScoped<ICriarCartaoUseCase, CriarCartaoUseCase>();
builder.Services.AddScoped<IAtualizarCartaoUseCase, AtualizarCartaoUseCase>();
builder.Services.AddScoped<IDeletarCartaoUseCase, DeletarCartaoUseCase>();
builder.Services.AddScoped<IValidator<CartaoCriarDto>, CartaoCriarDtoValidator>();

// Registra serviços para controle de períodos/meses de referência
builder.Services.AddScoped<IMesReferenciaRepository, MesReferenciaRepository>();
builder.Services.AddScoped<IAtualizarMesReferenciaUseCase, AtualizarMesReferenciaUseCase>();
builder.Services.AddScoped<ICriarMesReferenciaUseCase, CriarMesReferenciaUseCase>();
builder.Services.AddScoped<IDeletarMesReferenciaUseCase, DeletarMesReferenciaUseCase>();
builder.Services.AddScoped<IBuscarMesReferenciaUseCase, BuscarMesReferenciaUseCase>();
builder.Services.AddScoped<IListarMesReferenciaUseCase, ListarMesReferenciaUseCase>();
builder.Services.AddScoped<IValidator<MesReferenciaCriarDto>, MesReferenciaCriarDtoValidator>();

// Registra todos os use cases para gerenciamento de movimentacoes
builder.Services.AddScoped<IListarMovimentacoesReceitasUseCase, ListarMovimentacoesReceitasUseCase>();
builder.Services.AddScoped<IListarMovimentacoesUseCase, ListarMovimentacoesUseCase>();
builder.Services.AddScoped<IValidator<MovimentacaoCriarDto>, MovimentacaoCriarDtoValidtor>();
builder.Services.AddScoped<ICriarMovimentacaoUseCase, CriarMovimentacaoUseCase>();
builder.Services.AddScoped<IAtualizarMovimentacaoUseCase, AtualizarMovimentacaoUseCase>();
builder.Services.AddScoped<IBuscarMovimentacaoUseCase, BuscarMovimentacaoUseCase>();
builder.Services.AddScoped<IDeletarMovimentacaoUseCase, DeletarMovimentacaoUseCase>();

// Use cases para filtros específicos de movimentacoes
builder.Services.AddScoped<IMovimentacoesRepository, MovimentacoesRepository>();
builder.Services.AddScoped<IListarMovimentacoesPorDataUseCase, ListarMovimentacoesPorDataUseCase>();
builder.Services.AddScoped<IListarMovimentacoesPorContaBancariaUseCase, ListarMovimentacoesPorContaBancariaUseCase>();
builder.Services.AddScoped<IListarMovimentacoesPorCategoriaUseCase, ListarMovimentacoesPorCategoriaUseCase>();
builder.Services.AddScoped<IListarMovimentacoesPorTituloUseCase, ListarMovimentacoesPorTituloUseCase>();
builder.Services.AddScoped<IListarMovimentacoesPorCartaoUseCase, ListarMovimentacoesPorCartaoUseCase>();

// Registra use cases da tela de dashboard
builder.Services.AddScoped<IObterValorEmAbertoDespesasUseCase, ObterValorEmAbertoDespesasUseCase>();
builder.Services.AddScoped<IObterValorEmAbertoReceitasUseCase, ObterValorEmAbertoReceitasUseCase>();
builder.Services.AddScoped<IObterSaldoTotalUseCase, ObterSaldoTotalUseCase>();
builder.Services.AddScoped<IListarContasComSaldoTotalUseCase, ListarContasComSaldoTotalUseCase>();
builder.Services.AddScoped<IListarMovimentacoesEmAbertoUseCase, ListarMovimentacoesEmAbertoUseCase>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// Configura o AutoMapper para conversão entre DTOs e entidades
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(ContaBancariaMapper));
builder.Services.AddAutoMapper(typeof(CategoriaMapper));
builder.Services.AddAutoMapper(typeof(CartaoMapper));
builder.Services.AddAutoMapper(typeof(MesReferenciaMapper));


// CORS 
builder.Services.AddCors(options =>
{
    // POLÍTICA RESTRITIVA - Para ambiente de PRODUÇÃO
    // Define exatamente quais origens, métodos e headers são permitidos
    options.AddPolicy("PoliticaRestritiva", policy =>
    {
        policy.WithOrigins(
                // Adicione aqui os domínios reais do seu frontend
                "http://localhost:3000",     // React local (HTTP)
                "https://localhost:3000",    // React local (HTTPS)
                "http://localhost:4200",     // Angular local (HTTP)
                "https://localhost:4200",    // Angular local (HTTPS)
                "https://localhost:5173",    // Vite (React/Vue)
                "https://meuapp.com.br",     // Seu domínio de produção
                "https://www.meuapp.com.br"  // Variação com www
            )
            .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH") // Métodos HTTP permitidos
            .WithHeaders("Content-Type", "Authorization", "Accept", "X-Requested-With") // Headers permitidos
            .AllowCredentials(); // Permite envio de cookies e tokens de autenticação
    });

    // POLÍTICA PERMISSIVA - Para ambiente de DESENVOLVIMENTO
    // Permite qualquer origem, método e header (mais flexível para desenvolvimento)
    options.AddPolicy("PoliticaDesenvolvimento", policy =>
    {
        policy.AllowAnyOrigin()   // Qualquer origem pode acessar
            .AllowAnyMethod()     // Qualquer método HTTP
            .AllowAnyHeader();    // Qualquer header
        // IMPORTANTE: AllowCredentials() NÃO pode ser usado com AllowAnyOrigin()
    });

});

// Permite acessar informações do usuário logado em qualquer parte da aplicação
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

// Configura a autenticação usando JSON Web Tokens
builder.Services.AddAuthentication(options =>
{
    // Define JWT como esquema padrão de autenticação
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;            // Salva o token para uso posterior
        options.RequireHttpsMetadata = false; // Permite HTTP em desenvolvimento (mudar para true em produção)

        // Parâmetros de validação do token JWT
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,           // Valida quem emitiu o token
            ValidateAudience = true,         // Valida para quem o token foi emitido
            ValidateLifetime = true,         // Valida se o token não expirou
            ValidateIssuerSigningKey = true, // Valida a assinatura do token

            // Valores obtidos do appsettings.json
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero // Remove tolerância de tempo (mais seguro)
        };
    });

// Configura os controllers da API e opções de serialização JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Converte enums para string no JSON (em vez de números)
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Gera documentação automática da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();         // Gera a especificação OpenAPI
    app.UseSwaggerUI();       // Interface gráfica do Swagger
}

// Redireciona HTTP para HTTPS
app.UseHttpsRedirection();

// CORS deve vir CEDO no pipeline, antes da autenticação
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Página de erro detalhada em desenvolvimento
    app.UseCors("PoliticaDesenvolvimento"); // Política permissiva para desenvolvimento
}
else
{
    app.UseCors("PoliticaRestritiva"); // Política restritiva para produção
}

// middleware customizado para tratamento de erros
app.UseMiddleware<ControleFinanceiro.API.Middlewares.ExceptionMiddleware>();

// Authentication sempre antes de Authorization
app.UseAuthentication();
app.UseAuthorization();  

// Mapeia as rotas dos controllers
app.MapControllers();

app.Run();

// ===============================================
// RESUMO DAS CONFIGURAÇÕES CORS:
// ===============================================
/*
 * DESENVOLVIMENTO:
 * - Permite qualquer origem, método e header
 * - Mais flexível para testes
 * - NÃO permite credentials com AllowAnyOrigin
 * 
 * PRODUÇÃO:
 * - Apenas origens específicas são permitidas
 * - Métodos HTTP limitados
 * - Headers controlados
 * - Permite credentials (cookies, tokens)
 * 
 * ORDEM NO PIPELINE:
 * 1. HTTPS Redirection
 * 2. CORS (deve vir cedo)
 * 3. Middlewares customizados
 * 4. Authentication
 * 5. Authorization
 * 6. Controllers
 * 
 * DICAS IMPORTANTES:
 * - Sempre teste CORS em diferentes ambientes
 * - Use políticas restritivas em produção
 * - AllowCredentials + AllowAnyOrigin = ERRO
 * - CORS não é segurança, é convenção do browser
 */