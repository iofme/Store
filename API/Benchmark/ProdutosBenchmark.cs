using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Data;
using API.Models;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace API.Benchmark
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class ProdutosBenchmark
    {
        private AppDbContext _context;
        private ServiceProvider _serviceProvider;
        

        [GlobalSetup]
        public void Setup()
        {
            // Criar configuração para ler o appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var services = new ServiceCollection();

            // Adicionar a configuração ao serviço
            services.AddSingleton<IConfiguration>(configuration);

            // Obter a string de conexão da configuração
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;

            // Adicionar o contexto do banco de dados usando a string de conexão obtida
            services.AddDbContext<AppDbContext>(opt => 
                opt.UseSqlServer(connectionString)
            );
            
            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<AppDbContext>();
        }

        [Benchmark]
        public List<Produto> GetProdutosIEnumerable()
        {  
            IEnumerable<Produto> produtos = _context.Produtos.AsEnumerable();

            return produtos.ToList();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _context.Dispose();
            _serviceProvider.Dispose();
        }
    }
}