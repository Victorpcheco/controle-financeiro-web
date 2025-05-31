using System.Text;
using ControleFinanceiro.Application.Dtos;
using ControleFinanceiro.Application.Interfaces;
using ControleFinanceiro.Application.Interfaces.Categorias;
using ControleFinanceiro.Application.Interfaces.Contas;
using ControleFinanceiro.Application.Interfaces.Token;
using ControleFinanceiro.Application.Interfaces.Usuarios;
using ControleFinanceiro.Application.Mapping;
using ControleFinanceiro.Application.UseCases;
using ControleFinanceiro.Application.UseCases.Categorias;
using ControleFinanceiro.Application.UseCases.Contas;
using ControleFinanceiro.Application.UseCases.Usuarios;
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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILoginUsuario, LoginUsuario>();
builder.Services.AddScoped<IRegistroUsuario, RegistroUsuario>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IGerarToken, GerarToken>();
builder.Services.AddScoped<IGerarRefreshToken, GerarRefreshToken>();

builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
builder.Services.AddScoped<IListarContasBancarias, ListarContasBancarias>();
builder.Services.AddScoped<IObterContaBancaria, ObterContaBancaria>();
builder.Services.AddScoped<ICriarContaBancaria, CriarContaBancaria>();
builder.Services.AddScoped<IAtualizarContaBancaria, AtualizarContaBancaria>();
builder.Services.AddScoped<IDeletarContaBancaria, DeletarContaBancaria>();
builder.Services.AddScoped<IValidator<ContaRequest>, ContaRequestValidator>();
builder.Services.AddScoped<IValidator<ContaAtualizarRequest>, ContaAtualizarRequestValidator>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IListarCategorias, ListarCategorias>();
builder.Services.AddScoped<IObterCategoria, ObterCategoria>();
builder.Services.AddScoped<ICriarCategoria, CriarCategoria>();
builder.Services.AddScoped<IAtualizarCategoria, AtualizarCategoria>();
builder.Services.AddScoped<IDeletarCategoria, DeletarCategoria>();
builder.Services.AddScoped<IValidator<CategoriaRequest>, CategoriaRequestValidator>();

builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddAutoMapper(typeof(ContaProfile));
builder.Services.AddAutoMapper(typeof(CategoriaProfile));

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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ControleFinanceiro.API.Middlewares.ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
