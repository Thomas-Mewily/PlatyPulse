using Microsoft.EntityFrameworkCore;
using PlatyPulseAPI;
using PlatyPulseAPI.Value;

namespace PlatyPulseWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PlatyApp.InitJsonSerializerOptions();
            var j = 10.XP().ToJson();

            var builder = WebApplication.CreateBuilder(args);

            // Ajouter les services au conteneur

            // Ajoute les contrôleurs
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                PlatyApp.InitJsonSerializerOptions(options.JsonSerializerOptions);
            });


            // Configuration de la base de données SQLite
            builder.Services.AddDbContext<DataBaseCtx>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=PlatyPulse.db"));

            // Ajouter Swagger/OpenAPI pour la documentation de l'API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuration des CORS si nécessaire (exemple d'autorisation de toutes les origines pour les tests)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Initialisation de la base de données
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseCtx>();
                if (dbContext.Database.EnsureCreated())
                {
                    Console.WriteLine("Database was not found and has been created.");
                }
                else
                {
                    Console.WriteLine("Database already exists.");
                }
            }

            // Configuration du pipeline HTTP
            if (app.Environment.IsDevelopment())
            {
                // Swagger pour la documentation de l'API en mode développement
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirection HTTPS
            app.UseHttpsRedirection();

            // Ajout de CORS
            app.UseCors("AllowAll");

            // Middleware d'autorisation
            app.UseAuthorization();

            // Configuration des routes des contrôleurs
            app.MapControllers();

            // Démarrage de l'application
            app.Run();
        }
    }
}


