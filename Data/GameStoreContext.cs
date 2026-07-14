using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Games");
            entity.Property(g => g.Id).HasColumnName("Id");
            entity.Property(g => g.Name).HasColumnName("Name");
            entity.Property(g => g.GenreId).HasColumnName("GenreId");
            entity.Property(g => g.ReleaseDate).HasColumnName("ReleaseDate");
            
            entity.Property(g => g.Price)
                  .HasColumnName("Price")
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(g => g.Genre)
                  .WithMany()
                  .HasForeignKey(g => g.GenreId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genres");
            entity.Property(g => g.Id).HasColumnName("Id");
            entity.Property(g => g.Name).HasColumnName("Name");
        });
    }
}