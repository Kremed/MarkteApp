using System;
using System.Collections.Generic;
using MarketApp.Shered;
using Microsoft.EntityFrameworkCore;

namespace MarketApp.Backend;

public partial class CurrenciyDbContext : DbContext
{
    public CurrenciyDbContext()
    {
    }

    public CurrenciyDbContext(DbContextOptions<CurrenciyDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyPrice> CurrencyPrices { get; set; }

    public virtual DbSet<MarketingAd> MarketingAds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<MarketingAd>(entity =>
        {
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
