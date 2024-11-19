using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatyPulseAPI;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;
using System.Collections;
using System.Collections.Generic;

namespace PlatyPulseDB;

public class User : IdentifiableData
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }

    public User(string username, string hashedPassword, string salt = "perdu_au_jeu")
    {
        Username = username;
        HashedPassword = hashedPassword;
        Salt = salt;
    }
}

public class DataBaseCtx : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Challenge> Challenge { get; set; }
    public DbSet<ChallengeEntry> ChallengeEntry { get; set; }
    public DbSet<Quest> Quest { get; set; }
    public DbSet<QuestEntry> QuestEntry { get; set; }
    public DbSet<OwnedEmail> OwnedEmail { get; set; }
    public DbSet<OwnedPseudo> OwnedPseudo { get; set; }

    public DataBaseCtx() 
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=PlatyPulse.db");
    }

    private EntityTypeBuilder<T> AddTable<T>(ModelBuilder modelBuilder) where T : IdentifiableData 
    {
        modelBuilder.Entity<T>().HasKey(s => s.ID);
        return modelBuilder.Entity<T>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        AddTable<User>(modelBuilder);

        AddTable<OwnedEmail>(modelBuilder);
        AddTable<OwnedPseudo>(modelBuilder);

        AddTable<Challenge>(modelBuilder);
        AddTable<ChallengeEntry>(modelBuilder);

        AddTable<Quest>(modelBuilder).OwnsOne(q => q.Rank);
        AddTable<QuestEntry>(modelBuilder);

        // Value like
        modelBuilder.Owned<Pseudo>();
        modelBuilder.Owned<Email>();
        modelBuilder.Owned<Score>();
        modelBuilder.Owned<XP>();
        modelBuilder.Owned<List<Rank>>();
    }
}