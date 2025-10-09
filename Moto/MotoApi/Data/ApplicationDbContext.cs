using Microsoft.EntityFrameworkCore;
using MotoApi.Models;

namespace MotoApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Moto> Motos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.Identificador);
            
            entity.Property(e => e.Modelo)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Placa)
                .IsRequired()
                .HasMaxLength(20);

           // unique Placa
            entity.HasIndex(e => e.Placa)
                .IsUnique()
                .HasDatabaseName("IX_Motos_Placa_Unique");
        });
    }
}