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
    public virtual DbSet<Entregador> Entregadores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.Identificador);

            entity.HasIndex(e => e.Placa, "IX_Motos_Placa").IsUnique();

            entity.Property(e => e.Identificador).HasColumnType("varchar(50)"); 
            entity.Property(e => e.Modelo).HasMaxLength(100);
            entity.Property(e => e.Placa).HasMaxLength(8);
        });
        
        modelBuilder.Entity<Entregador>(entity =>
        {
            entity.HasKey(e => e.Identificador);

            entity.HasIndex(e => e.Cnpj, "IX_Entregadores_Cnpj").IsUnique();
            entity.HasIndex(e => e.NumeroCnh, "IX_Entregadores_NumeroCnh").IsUnique();

            entity.Property(e => e.Identificador).HasColumnType("varchar(50)"); 
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Cnpj).HasMaxLength(14);
            entity.Property(e => e.NumeroCnh).HasMaxLength(20);
            entity.Property(e => e.TipoCnh).HasMaxLength(3);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
