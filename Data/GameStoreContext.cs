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
            entity.Property(g => g.Price)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(g => g.Genre)
                  .WithMany()
                  .HasForeignKey(g => g.GenreId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}