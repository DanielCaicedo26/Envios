using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Entity.Model;
using Entity.Model.Base;

namespace Entity.Context
{
    /// <summary>
    /// Contexto de base de datos específico para auditoría y logs
    /// </summary>
    public class AuditDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public AuditDbContext(DbContextOptions<AuditDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // ← ESTA LÍNEA ES CRUCIAL - Define el DbSet
        public DbSet<ConsoleLog> ConsoleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración específica para ConsoleLog
            modelBuilder.Entity<ConsoleLog>(entity =>
            {
                entity.ToTable("ConsoleLogs");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RecordId)
                    .IsRequired();

                entity.Property(e => e.OperationType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.OldValues)
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.NewValues)
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100);

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(45);

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(500);

                entity.Property(e => e.Timestamp)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnType("nvarchar(max)");

                // Índices para mejorar las consultas
                entity.HasIndex(e => e.TableName)
                    .HasDatabaseName("IX_ConsoleLogs_TableName");

                entity.HasIndex(e => e.RecordId)
                    .HasDatabaseName("IX_ConsoleLogs_RecordId");

                entity.HasIndex(e => e.Timestamp)
                    .HasDatabaseName("IX_ConsoleLogs_Timestamp");

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_ConsoleLogs_UserId");

                entity.HasIndex(e => new { e.TableName, e.RecordId })
                    .HasDatabaseName("IX_ConsoleLogs_Table_Record");
            });

            // Configuración de propiedades base solo para ConsoleLog
            modelBuilder.Entity<ConsoleLog>()
                .Property("CreatedAt")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ConsoleLog>()
                .Property("UpdatedAt")
                .IsRequired(false);

            modelBuilder.Entity<ConsoleLog>()
                .Property("DeleteAt")
                .IsRequired(false);

            modelBuilder.Entity<ConsoleLog>()
                .Property("Status")
                .HasDefaultValue(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}