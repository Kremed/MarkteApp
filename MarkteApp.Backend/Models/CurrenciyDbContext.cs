using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MarkteApp.Backend.Models;

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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=MarketDBConniction");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<CurrencyPrice>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.RecordTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<MarketingAd>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
