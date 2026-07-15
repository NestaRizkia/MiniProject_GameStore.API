using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Games");
            entity.Property(g => g.Id).HasColumnName("Id");
            entity.Property(g => g.Name).HasColumnName("Name");
            entity.Property(g => g.GenreId).HasColumnName("GenreId");
            entity.Property(g => g.ReleaseDate).HasColumnName("ReleaseDate");
            entity.Property(g => g.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(g => g.UpdatedAt).HasColumnName("UpdatedAt");
            
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
            entity.Property(g => g.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(g => g.UpdatedAt).HasColumnName("UpdatedAt");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(u => u.Id).HasColumnName("Id");
            entity.Property(u => u.Username).HasColumnName("Username");
            entity.Property(u => u.Email).HasColumnName("Email");
            entity.Property(u => u.PasswordHash).HasColumnName("PasswordHash");
            entity.Property(u => u.Role).HasColumnName("Role");
            entity.Property(u => u.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(u => u.UpdatedAt).HasColumnName("UpdatedAt");

            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Game || e.Entity is Genre || e.Entity is User);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is Game game)
                {
                    game.CreatedAt = DateTime.UtcNow;
                    game.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Genre genre)
                {
                    genre.CreatedAt = DateTime.UtcNow;
                    genre.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is User user)
                {
                    user.CreatedAt = DateTime.UtcNow;
                    user.UpdatedAt = DateTime.UtcNow;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is Game game)
                {
                    game.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Genre genre)
                {
                    genre.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}