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
using ControleFinanceiro.Application.UseCases.Despesas.AtualizarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.BuscarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.CriarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.DeletarDespesa;
using ControleFinanceiro.Application.UseCases.Despesas.ListarDespesa;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.AtualizarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.BuscarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.CriarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.DeletarMesReferencia;
using ControleFinanceiro.Application.UseCases.MesDeReferencia.ListarMesReferencia;
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

// config string conexão
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// useCases / repositories / validators / mappers
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILoginUsuarioUseCase, LoginUsuarioUseCase>();
builder.Services.AddScoped<IRegistroUsuarioUseCase, RegistroUsuarioUseCase>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
builder.Services.AddScoped<IGerarToken, GerarToken>();
builder.Services.AddScoped<IGerarRefreshToken, GerarRefreshToken>();

builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
builder.Services.AddScoped<IListarContasBancariasUseCase, ListarContasBancariasUseCase>();
builder.Services.AddScoped<IBuscarContaBancariaUseCase, BuscarContaBancariaUseCase>();
builder.Services.AddScoped<ICriarContaBancariaUseCase, CriarContaBancariaUseCase>();
builder.Services.AddScoped<IAtualizarContaBancariaUseCase, AtualizarContaBancariaUseCaseUseCase>();
builder.Services.AddScoped<IDeletarContaBancariaUseCase, DeletarContaBancariaUseCase>();
builder.Services.AddScoped<IValidator<ContaBancariaCriarDto>, ContaBancariaCriarDtoValidator>();
builder.Services.AddScoped<IValidator<ContaBancariaAtualizarDto>, ContaBancariaAtualizarDtoValidator>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IListarCategoriasUseCase, ListarCategoriasUseCase>();
builder.Services.AddScoped<IBuscarCategoriaUseCase, BuscarCategoriaUseCase>();
builder.Services.AddScoped<ICriarCategoriaUseCase, CriarCategoriaUseCase>();
builder.Services.AddScoped<IAtualizarCategoriaUseCase, AtualizarCategoriaUseCase>();
builder.Services.AddScoped<IDeletarCategoriaUseCase, DeletarCategoriaUseCase>();
builder.Services.AddScoped<IValidator<CategoriaCriarDto>, CategoriaRequestValidator>();

builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
builder.Services.AddScoped<IListarCartaoPaginadoUseCase, ListarCartaoPaginadoUseCase>();
builder.Services.AddScoped<IBuscarCartaoUseCase, BuscarCartaoUseCase>();
builder.Services.AddScoped<ICriarCartaoUseCase, CriarCartaoUseCase>();
builder.Services.AddScoped<IAtualizarCartaoUseCase, AtualizarCartaoUseCase>();
builder.Services.AddScoped<IDeletarCartaoUseCase, DeletarCartaoUseCase>();
builder.Services.AddScoped<IValidator<CartaoCriarDto>, CartaoCriarDtoValidator>();

builder.Services.AddScoped<IMesReferenciaRepository, MesReferenciaRepository>();
builder.Services.AddScoped<IAtualizarMesReferenciaUseCase, AtualizarMesReferenciaUseCase>();
builder.Services.AddScoped<ICriarMesReferenciaUseCase, CriarMesReferenciaUseCase>();
builder.Services.AddScoped<IDeletarMesReferenciaUseCase, DeletarMesReferenciaUseCase>();
builder.Services.AddScoped<IBuscarMesReferenciaUseCase, BuscarMesReferenciaUseCase>();
builder.Services.AddScoped<IListarMesReferenciaUseCase, ListarMesReferenciaUseCase>();
builder.Services.AddScoped<IValidator<MesReferenciaCriarDto>, MesReferenciaCriarDtoValidator>();

builder.Services.AddScoped<IListarDespesasUseCase, ListarDespesasUseCase>();
builder.Services.AddScoped<IValidator<DespesaCriarDto>, DespesaCriaDtoValidtor>();
builder.Services.AddScoped<ICriarDespesaUseCase, CriarDespesaUseCase>();
builder.Services.AddScoped<IDespesaRepository, DespesaRepository>();
builder.Services.AddScoped<IAtualizarDespesaUseCase, AtualizarDespesaUseCase>();
builder.Services.AddScoped<IBuscarDespesaUseCase, BuscarDespesaUseCase>();
builder.Services.AddScoped<IDeletarDespesaUseCase, DeletarDespesaUseCase>();


builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddAutoMapper(typeof(ContaBancariaMapper));
builder.Services.AddAutoMapper(typeof(CategoriaMapper));
builder.Services.AddAutoMapper(typeof(CartaoMapper));
builder.Services.AddAutoMapper(typeof(MesReferenciaMapper));


// Configurações de validação de usuarios
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false; 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"], 
            ValidAudience = builder.Configuration["JWT:Audience"], 
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!) 
            ),
            ClockSkew = TimeSpan.Zero 
        };
    });

// Configura os controllers e a serialização de enums como string no JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ControleFinanceiro.API.Middlewares.ExceptionMiddleware>(); // uso do middleware de exceção
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
