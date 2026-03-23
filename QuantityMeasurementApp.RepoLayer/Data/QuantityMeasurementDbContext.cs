using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Data
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(
            DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<UserEntity>                Users                { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("quantity_measurements");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OperandOne).HasMaxLength(200);
                entity.Property(e => e.OperandTwo).HasMaxLength(200);
                entity.Property(e => e.Result).HasMaxLength(200);
                entity.Property(e => e.MeasureType).HasMaxLength(50);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}