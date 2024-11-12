using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace PlatyDataBase.Data
{
    public class PlatyDataBaseContextFactory : IDesignTimeDbContextFactory<PlatyDataBaseContext>
    {
        public PlatyDataBaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlatyDataBaseContext>();

            // Charge la configuration depuis appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlite(configuration.GetConnectionString("UserServiceContext"));

            return new PlatyDataBaseContext(optionsBuilder.Options);
        }
    }
}    
