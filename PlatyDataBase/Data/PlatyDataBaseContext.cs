using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatyDataBase.Entities;


namespace PlatyDataBase.Data
{
    public class PlatyDataBaseContext : DbContext
    {
        public PlatyDataBaseContext(DbContextOptions<PlatyDataBaseContext> options)
            : base(options)
        {
        }

        // Définis les DbSet pour tes entités ici, par exemple :
        public DbSet<User> Users { get; set; }
        public DbSet<Quest> Quests { get; set; }
    }
}
