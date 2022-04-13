using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CarInspectionManagment.Business.Persistence.Entities
{
    public partial class CarInspectionManagementContext : DbContext
    {
        public CarInspectionManagementContext()
        {
        }

        public CarInspectionManagementContext(DbContextOptions<CarInspectionManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarInspection> CarInspections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=CarInspectionManagement ;Username=cars;Password=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<CarInspection>(entity =>
            {
                entity.ToTable("CarInspection ");

                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.DateOfCreation).HasColumnType("date");

                entity.Property(e => e.Reason)
                    .HasMaxLength(50)
                    .HasColumnName("Reason ");

                entity.Property(e => e.Vinnumber)
                    .HasMaxLength(100)
                    .HasColumnName("VINNumber ");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
