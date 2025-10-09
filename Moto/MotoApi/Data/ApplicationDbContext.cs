using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Moto> Motos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.Identificador);

            entity.HasIndex(e => e.Placa, "IX_Motos_Placa").IsUnique();

            entity.Property(e => e.Identificador).HasColumnType("varchar(50)"); // Define explicitamente como varchar
            entity.Property(e => e.Modelo).HasMaxLength(100);
            entity.Property(e => e.Placa).HasMaxLength(8);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
