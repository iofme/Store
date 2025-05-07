using System.Text.Json.Serialization;
using API.Benchmark;
using API.Controllers;
using API.Data;
using API.Extensions;
using API.Filtros;
using API.Interface;
using API.Logging;
using API.Repository;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => 
    {
     opt.Filters.Add(typeof(ApiExceptionFilter));
    }
    )
        .AddJsonOptions(opt => 
        opt.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

if (args.Contains("--benchmark"))
{
    // Executar benchmarks
    BenchmarkRunner.Run<ProdutosBenchmark>();
    return; // Encerra a aplicação após os benchmarks
}

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig 
{
    LogLevel = LogLevel.Information,
}));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
            options.SwaggerEndpoint("/openapi/v1.json", "weather api"));
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
