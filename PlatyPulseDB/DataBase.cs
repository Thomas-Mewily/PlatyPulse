using BetterCSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.Collections.Generic;

namespace PlatyPulseWebAPI;

public class DataBaseCtx(DbContextOptions<DataBaseCtx> options) : DbContext(options)
{
    //public static DataBaseCtx Instance { get; } = new DataBaseCtx();

    public DbSet<User> Account { get; set; } = null!;


    public DbSet<User> User { get; set; } = null!;

    public DbSet<Challenge> Challenge { get; set; } = null!;
    public DbSet<ChallengeEntry> ChallengeEntry { get; set; } = null!;

    public DbSet<Quest> Quest { get; set; } = null!;
    public DbSet<QuestEntry> QuestEntry { get; set; } = null!;

    public User? AccountFromEmail(Email email) { return Account.FirstOrDefault(e => e.Email == email); }
    public bool EmailAvailable(Email email) { return AccountFromEmail(email) == null; }

    //public DataBaseCtx() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            "Db was not init".Panic();
        }
    }

    private EntityTypeBuilder<T> AddTable<T>(ModelBuilder modelBuilder) where T : IdentifiableByID
    {
        modelBuilder.Entity<T>().HasKey(s => s.ID);
        return modelBuilder.Entity<T>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = AddTable<User>(modelBuilder).HasIndex(e => e.Email).IsUnique();

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email)
                  .HasConversion(
                      v => v.Address,          
                      v => v.ToEmail());
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Pseudo)
                  .HasConversion(
                      v => v.Name,
                      v => v.ToPseudo());
        });


        AddTable<User>(modelBuilder);

        AddTable<Challenge>(modelBuilder);
        AddTable<ChallengeEntry>(modelBuilder);

        AddTable<Quest>(modelBuilder).OwnsMany(q => q.Rank, rank => 
        { 
            rank.WithOwner();
        });
        AddTable<QuestEntry>(modelBuilder);

        // Value like
        modelBuilder.Owned<Pseudo>();
        modelBuilder.Owned<Email>();
        modelBuilder.Owned<Score>();
        modelBuilder.Owned<XP>();
        modelBuilder.Owned<List<Rank>>();
        modelBuilder.Owned<Rank>();
    }

    public void PrintSchema() 
    {
        // Get the model metadata (table and column information)
        var model = Model;

        // Loop through each entity (table)
        foreach (var entityType in model.GetEntityTypes())
        {
            Console.WriteLine($"Table: {entityType.GetTableName()}");

            // Loop through each property (column) in the entity
            foreach (var property in entityType.GetProperties())
            {
                Console.WriteLine($"    Column: {property.Name}, Type: {property.ClrType.Name}");
            }

            Console.WriteLine();
        }
    }
}