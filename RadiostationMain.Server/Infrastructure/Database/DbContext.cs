using Microsoft.EntityFrameworkCore;
using RadiostationMain.Server.Infrastructure.Models;

namespace RadiostationMain.Server.Infrastructure.Database
{
    public class RadiostationContext : DbContext
    {
        public RadiostationContext(DbContextOptions<RadiostationContext> options) : base(options) { }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Station> Stations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasOne(t => t.Station)
                      .WithMany(s => s.Tracks)
                      .HasForeignKey(t => t.StationId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.Property(t => t.Title).IsRequired();
                entity.Property(t => t.Artist).IsRequired();
                entity.Property(t => t.FilePath).IsRequired();
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.Property(s => s.Name).IsRequired();
            });
        }

    }
}
