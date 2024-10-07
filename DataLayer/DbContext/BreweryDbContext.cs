using Brewery.Domain;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class BreweryDbContext : DbContext
{
    public DbSet<Beer> Players { get; set; }
    public DbSet<WholeSaler> WholeSalers { get; set; }
    public DbSet<Brewer> Brewers { get; set; }
    public DbSet<WholeSaler> BeerStock { get; set; }

    public BreweryDbContext() : base()
    {
    }

    public BreweryDbContext(DbContextOptions<BreweryDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ReadersDB");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beer>().HasOne(b => b.Brewer);

        modelBuilder.Entity<Brewer>().HasMany(b => b.Beers);
    }
}