using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Carregar configuração do appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Use a string de conexão do seu appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Substitua UseSqlServer pelo provedor que você está usando
            builder.UseSqlServer(connectionString);
            
            return new AppDbContext(builder.Options);
        }
    }
}