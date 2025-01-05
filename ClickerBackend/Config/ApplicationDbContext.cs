using ClickerBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClickerBackend.Config
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Upgrade> Upgrade { get; set; }

        public ApplicationDbContext() : base() { }
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
