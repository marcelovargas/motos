using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data;

public partial class ApplicationDbContext : DbContext
{   
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Moto> Motos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.Identificador);

            entity.HasIndex(e => e.Placa, "IX_Motos_Placa").IsUnique();

            entity.Property(e => e.Modelo).HasMaxLength(100);
            entity.Property(e => e.Placa).HasMaxLength(7);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
