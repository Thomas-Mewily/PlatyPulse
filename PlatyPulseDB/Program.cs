using Microsoft.EntityFrameworkCore;
using PlatyPulseAPI;
using PlatyPulseAPI.Data;

namespace PlatyPulseDB;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var db_ctx = new MyDatabaseContext();

        db_ctx.Database.EnsureCreated();
        //db_ctx.Add(new User(new Pseudo("17h21"), DateTime.Now, DateTime.Now, 100.XP()));
        db_ctx.SaveChanges();

        foreach(var i in db_ctx.Set<IdentifiableData>())
        {
            Console.WriteLine(i);
        } 
    }
}

public class MyDatabaseContext : DbContext
{
    public DbSet<IdentifiableData> Db { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=PlatyPulseOk.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Key
        modelBuilder.Entity<IdentifiableData>().HasKey(s => s.ID);


        // Table
        modelBuilder.Entity<User>();

        modelBuilder.Entity<Challenge>();
        modelBuilder.Entity<ChallengeEntry>();

        modelBuilder.Entity<Quest>().OwnsOne(q => q.Rank);
        modelBuilder.Entity<QuestEntry>();

        // Value like
        modelBuilder.Owned<Pseudo>();
        modelBuilder.Owned<Score>();
        modelBuilder.Owned<XP>();
        modelBuilder.Owned<List<Rank>>();
    }
}