using api.Entities;
using Microsoft.EntityFrameworkCore;
namespace api.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<TokboxSession> Session { get; set; }
        public DbSet<TokboxTokens> Tokens { get; set; }
    }
}