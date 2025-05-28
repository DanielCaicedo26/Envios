using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Entity.Model;

namespace Entity.Context
{
    /// <summary>
    /// Contexto de base de datos específico SOLO para auditoría
    /// </summary>
    public class AuditDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AuditDbContext(DbContextOptions<AuditDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        // SOLO el DbSet de ConsoleLog - Sin otras entidades
        public DbSet<ConsoleLog> ConsoleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NO llamar a base.OnModelCreating() para evitar heredar configuraciones

            // Configuración SOLO para ConsoleLog
            modelBuilder.Entity<ConsoleLog>(entity =>
            {
                entity.ToTable("ConsoleLogs");
                entity.HasKey(e => e.Id);

                // Configurar propiedades específicas
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

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

                // Propiedades de BaseEntity
                entity.Property(e => e.Status)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false);

                entity.Property(e => e.DeleteAt)
                    .IsRequired(false);

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configuraciones adicionales si son necesarias
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}