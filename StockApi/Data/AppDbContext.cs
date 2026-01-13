using Microsoft.EntityFrameworkCore;
using StockApi.Modals;


namespace VidepGame.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> products => Set<Product>(); // links queries to database, This is really just a table in the database
        public DbSet<StockLine> stockLines => Set<StockLine>(); // links queries to database, This is really just a table in the database
        public DbSet<User> users => Set<User>(); // links queries to database, This is really just a table in the database
        public DbSet<Session> sessions => Set<Session>(); // links queries to database, This is really just a table in the database
    }
}
