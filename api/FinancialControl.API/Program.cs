using FinancialControl.Application.DTOs;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.Mapping;
using FinancialControl.Application.Services;
using FinancialControl.Application.Validators;
using FinancialControl.Domain.Interfaces;
using FinancialControl.Infrastructure.Authentication;
using FinancialControl.Infrastructure.Data;
using FinancialControl.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura o contexto do Entity Framework para usar SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra repositórios e serviços para injeção de dependência
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IValidator<UsuarioRequestDto>, UsuarioRegistroDtoValidator>();
builder.Services.AddScoped<IValidator<UsuarioLoginDto>, UsuarioLoginDtoValidator>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IValidator<CategoriaRequestDto>, CategoriaRequestDtoValidator>();

builder.Services.AddScoped<IContaBancariaService, ContaBancariaService>();
builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
builder.Services.AddAutoMapper(typeof(ContaBancariaProfile));
builder.Services.AddScoped<IValidator<ContaBancariaRequestDto>, ContaBancariaRequestDtoValidator>();

// Configura os controllers e a serialização de enums como string no JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Adiciona suporte ao Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona HTTP para HTTPS
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseMiddleware<FinancialControl.API.Middlewares.ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
