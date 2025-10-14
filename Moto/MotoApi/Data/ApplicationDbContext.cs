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
    public virtual DbSet<Locacao> Locacoes { get; set; }
    public virtual DbSet<EventoMoto2024> EventosMoto2024 { get; set; }

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

        modelBuilder.Entity<Locacao>(entity =>
        {
            entity.HasKey(e => e.Identificador);

            entity.HasIndex(e => e.EntregadorId, "IX_Locacoes_EntregadorId");
            entity.HasIndex(e => e.MotoId, "IX_Locacoes_MotoId");

            entity.Property(e => e.Identificador).HasColumnType("varchar(50)");
            entity.Property(e => e.EntregadorId).HasColumnType("varchar(50)");
            entity.Property(e => e.MotoId).HasColumnType("varchar(50)");

            entity.HasOne(d => d.Entregador)
                .WithMany(p => p.Locacoes)
                .HasForeignKey(d => d.EntregadorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Moto)
                .WithMany(p => p.Locacoes)
                .HasForeignKey(d => d.MotoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EventoMoto2024>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Identificador).HasColumnType("varchar(50)");
            entity.Property(e => e.Modelo).HasMaxLength(100);
            entity.Property(e => e.Placa).HasMaxLength(8);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
