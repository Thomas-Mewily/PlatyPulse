using BetterCSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatyPulseAPI.Data;
using PlatyPulseAPI.Value;

namespace PlatyPulseWebAPI;

public class DataBaseCtx : DbContext
{
    //public static DataBaseCtx Instance { get; } = new DataBaseCtx();

    public DbSet<Account> Account { get; set; }


    public DbSet<User> User { get; set; }

    public DbSet<OwnedEmail> OwnedEmail { get; set; }
    public DbSet<OwnedPseudo> OwnedPseudo { get; set; }

    public DbSet<Challenge> Challenge { get; set; }
    public DbSet<ChallengeEntry> ChallengeEntry { get; set; }

    public DbSet<Quest> Quest { get; set; }
    public DbSet<QuestEntry> QuestEntry { get; set; }

    public Account? AccountFromUserName(string username) { return Account.FirstOrDefault(e => e.Username == username); }
    public bool UserNameAvailable(string username) { return AccountFromUserName(username) == null; }

    public DataBaseCtx(DbContextOptions<DataBaseCtx> options) : base(options) { }
    //public DataBaseCtx() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            "Db was not init".Panic();
        }
    }

    private EntityTypeBuilder<T> AddTable<T>(ModelBuilder modelBuilder) where T : IdentifiableData
    {
        modelBuilder.Entity<T>().HasKey(s => s.ID);
        return modelBuilder.Entity<T>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table
        AddTable<Account>(modelBuilder).HasIndex(e => e.Username).IsUnique();

        AddTable<User>(modelBuilder);

        AddTable<OwnedEmail>(modelBuilder);
        AddTable<OwnedPseudo>(modelBuilder);

        AddTable<Challenge>(modelBuilder);
        AddTable<ChallengeEntry>(modelBuilder);

        AddTable<Quest>(modelBuilder).OwnsMany(q => q.Rank, rank => rank.WithOwner()); // WithOne(r => r.Qu) .OwnsOne(q => q.Rank);
        AddTable<QuestEntry>(modelBuilder);

        // Value like
        modelBuilder.Owned<Pseudo>();
        modelBuilder.Owned<Email>();
        modelBuilder.Owned<Score>();
        modelBuilder.Owned<XP>();
        modelBuilder.Owned<List<Rank>>();
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