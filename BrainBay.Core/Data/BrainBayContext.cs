using Microsoft.EntityFrameworkCore;
using BrainBay.Core.Models;

namespace BrainBay.Core.Data
{
    public class BrainBayContext : DbContext
    {
        public BrainBayContext(DbContextOptions<BrainBayContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.OwnsOne(c => c.Origin, origin =>
                {
                    origin.Property(o => o.Name).HasColumnName("Origin_Name");
                    origin.Property(o => o.Url).HasColumnName("Origin_Url");
                });

                entity.OwnsOne(c => c.Location, location =>
                {
                    location.Property(l => l.Name).HasColumnName("Location_Name");
                    location.Property(l => l.Url).HasColumnName("Location_Url");
                });

                entity.Property(c => c.Episode)
                    .HasConversion(
                        v => string.Join(",", v ?? Array.Empty<string>()),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            });
        }
    }
} 