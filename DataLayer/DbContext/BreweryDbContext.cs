using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DbContext;

public class BreweryDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BreweryDbContext()
    {
    }

    public BreweryDbContext(DbContextOptions<BreweryDbContext> options) : base(options)
    {
    }

    public DbSet<Brewery> Breweries { get; set; }
    public DbSet<Beer> Beers { get; set; }
    public DbSet<Wholesaler> Wholesalers { get; set; }
    public DbSet<WholesalerStock> WholesalerStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ReadersDB");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WholesalerStock>()
            .HasKey(e => new { e.WholesalerId, e.BeerId });

        modelBuilder.Entity<Brewery>()
            .HasMany(b => b.Beers)
            .WithOne(b => b.Brewery)
            .HasForeignKey(b => b.BreweryId);

        modelBuilder.Entity<Wholesaler>()
            .HasMany(w => w.Stocks)
            .WithOne(ws => ws.Wholesaler)
            .HasForeignKey(ws => ws.WholesalerId).OnDelete(DeleteBehavior.Cascade);
    }
}