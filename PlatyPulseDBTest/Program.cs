using PlatyPulseAPI.Value;
using PlatyPulseDB;

namespace PlatyPulseDBTest;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var db_ctx = new DataBaseCtx();
        db_ctx.Database.EnsureCreated();

        db_ctx.Add("cool".ToOwnedPseudo());

        db_ctx.SaveChanges();


        foreach (var p in db_ctx.OwnedPseudo)
        {
            Console.WriteLine(p);
        }
    }
}