using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PlatyPulseAPI;
using PlatyPulseAPI.Value;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace PlatyPulseWebAPI
{
    public class Program
    {



        public static void Main(string[] args)
        {
            PlatyApp.InitJsonSerializerOptions();

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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret.TopSecretToken())),  // Your secret key used to sign the JWT
                    ClockSkew = TimeSpan.Zero  // Optional: Remove clock skew
                };
            });

            builder.Services.AddSwaggerGen(options =>
            {
                // Add a security definition for the Bearer token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });

                // Add security requirement to ensure the Bearer token is included in the Swagger UI
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
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


