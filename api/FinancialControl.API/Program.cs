using System;
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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionando Serviços e Repositórios
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(UsuarioProfile));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IValidator<UsuarioRegistroDto>, UsuarioRegistroDtoValidator>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IValidator<UsuarioLoginDto>, UsuarioLoginDtoValidator>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IValidator<CategoriaRequestDto>, CategoriaRequestDtoValidator>();
builder.Services.AddScoped<IContaBancariaService, ContaBancariaService>();
builder.Services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
builder.Services.AddAutoMapper(typeof(ContaBancariaProfile));
builder.Services.AddScoped<IValidator<ContaBancariaRequestDto>, ContaBancariaRequestDtoValidator>();

builder.Services.AddControllers() // Converte enum para string
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    }); 

// Adicionando configuração do Cors (frontend)
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin"); 
app.UseMiddleware<FinancialControl.API.Middlewares.ExceptionMiddleware>(); // Tratamento de exceções
app.UseAuthorization();
app.MapControllers();
app.Run();
