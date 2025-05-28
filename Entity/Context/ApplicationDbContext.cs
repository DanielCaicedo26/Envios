using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Entity.Model;
using Entity.Model.Base;
using System.Data;
using Dapper;
using System.Linq.Expressions;

namespace Entity.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        // ========================= DBSETS - TODAS LAS ENTIDADES =========================

        // Entidades principales del sistema de usuarios y roles
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolUser> RolUsers { get; set; }

        // Entidades geográficas y de ubicación
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }

        // Entidades de personas y relacionados
        public DbSet<Person> People { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // Entidades del sistema de módulos y formularios
        public DbSet<Module> Modules { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<ModuleForm> ModuleForms { get; set; }

        // Entidades de permisos y autorizaciones
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolFormPermission> RolFormPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========================= CONFIGURACIÓN DE RELACIONES =========================

            // Configuración de la relación muchos-a-muchos RolUser
            modelBuilder.Entity<RolUser>()
                .HasKey(ru => new { ru.UserId, ru.RolId }); // Clave compuesta

            modelBuilder.Entity<RolUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.RolUsers)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<RolUser>()
                .HasOne(ru => ru.Rol)
                .WithMany(r => r.RolUsers)
                .HasForeignKey(ru => ru.RolId);

            // ========================= CONFIGURACIÓN GEOGRÁFICA =========================

            // Configuración Country -> Department (1:N)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Country)
                .WithMany(c => c.Departments)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración Department -> City (1:N)
            modelBuilder.Entity<City>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Cities)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración City -> Neighborhood (1:N)
            modelBuilder.Entity<Neighborhood>()
                .HasOne(n => n.City)
                .WithMany(c => c.Neighborhoods)
                .HasForeignKey(n => n.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE PERSONAS =========================

            // Configuración Person - relaciones geográficas
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Country)
                .WithMany(c => c.People)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Department)
                .WithMany(d => d.People)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.City)
                .WithMany(c => c.People)
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Neighborhood)
                .WithMany(n => n.People)
                .HasForeignKey(p => p.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE CLIENTES =========================

            // Configuración Client - relación con Person (1:1)
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Person)
                .WithOne(p => p.Client)
                .HasForeignKey<Client>(c => c.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración Client - relaciones geográficas
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Country)
                .WithMany(co => co.Clients)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Clients)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.City)
                .WithMany(ci => ci.Clients)
                .HasForeignKey(c => c.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Neighborhood)
                .WithMany(n => n.Clients)
                .HasForeignKey(c => c.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE PROVEEDORES =========================

            // Configuración Provider - relación con Person (1:1)
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Person)
                .WithOne(pe => pe.Provider)
                .HasForeignKey<Provider>(p => p.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración Provider - relaciones geográficas
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Country)
                .WithMany(c => c.Providers)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Providers)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.City)
                .WithMany(c => c.Providers)
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Neighborhood)
                .WithMany(n => n.Providers)
                .HasForeignKey(p => p.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE EMPLEADOS =========================

            // Configuración Employee - relación con Person (1:1)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Person)
                .WithOne(p => p.Employee)
                .HasForeignKey<Employee>(e => e.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración Employee - relaciones geográficas
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Country)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.City)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Neighborhood)
                .WithMany(n => n.Employees)
                .HasForeignKey(e => e.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración Employee - relación supervisor/subordinado (auto-referencia)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany(s => s.Subordinates)
                .HasForeignKey(e => e.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE MÓDULOS Y FORMULARIOS =========================

            // Configuración ModuleForm - relación muchos-a-muchos
            modelBuilder.Entity<ModuleForm>()
                .HasKey(mf => new { mf.ModuleId, mf.FormId });

            modelBuilder.Entity<ModuleForm>()
                .HasOne(mf => mf.Module)
                .WithMany()
                .HasForeignKey(mf => mf.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ModuleForm>()
                .HasOne(mf => mf.Form)
                .WithMany(f => f.ModuleForm)
                .HasForeignKey(mf => mf.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE PERMISOS =========================

            // Configuración RolFormPermission - relación compleja
            modelBuilder.Entity<RolFormPermission>()
                .HasKey(rfp => new { rfp.RolId, rfp.FormId, rfp.PermissionId });

            modelBuilder.Entity<RolFormPermission>()
                .HasOne(rfp => rfp.Rol)
                .WithMany()
                .HasForeignKey(rfp => rfp.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolFormPermission>()
                .HasOne(rfp => rfp.Permission)
                .WithMany(p => p.RolPermissions)
                .HasForeignKey(rfp => rfp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========================= CONFIGURACIÓN DE PROPIEDADES BASE =========================

            // Configuración para todas las entidades que heredan de BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                // Configurar CreatedAt para que no sea nullable
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .IsRequired();

                // Configurar UpdatedAt y DeleteAt como nullable
                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .IsRequired(false);

                modelBuilder.Entity(entityType.ClrType)
                    .Property("DeleteAt")
                    .IsRequired(false);

                // Configurar Status con un valor predeterminado de true
                modelBuilder.Entity(entityType.ClrType)
                    .Property("Status")
                    .HasDefaultValue(true);
            }

            // ========================= CONFIGURACIONES ESPECÍFICAS =========================

            // Configuración de precisión decimal
            modelBuilder.Entity<Client>()
                .Property(c => c.CreditLimit)
                .HasPrecision(18, 2);

            // Configuración de índices únicos
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[Status] = 1"); // Solo usuarios activos

            modelBuilder.Entity<Person>()
                .HasIndex(p => new { p.DocumentType, p.DocumentNumber })
                .IsUnique()
                .HasFilter("[Status] = 1"); // Solo personas activas

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.ClientCode)
                .IsUnique()
                .HasFilter("[Status] = 1");

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.TaxId)
                .IsUnique()
                .HasFilter("[Status] = 1");

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.ProviderCode)
                .IsUnique()
                .HasFilter("[Status] = 1");

            modelBuilder.Entity<Provider>()
                .HasIndex(p => p.TaxId)
                .IsUnique()
                .HasFilter("[Status] = 1");

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique()
                .HasFilter("[Status] = 1");

            // Configuración de longitud de campos
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(255);

            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasMaxLength(50);

            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<Person>()
                .Property(p => p.DocumentNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<Country>()
                .Property(c => c.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<City>()
                .Property(c => c.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Neighborhood>()
                .Property(n => n.Name)
                .HasMaxLength(100);
        }

        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Otras configuraciones adicionales pueden ir aquí
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        /// <summary>
        /// Guarda los cambios en la base de datos, asegurando la auditoría antes de persistir los datos.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guarda los cambios en la base de datos de manera asíncrona, asegurando la auditoría antes de la persistencia.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indica si se deben aceptar todos los cambios en caso de éxito.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación.</param>
        /// <returns>Número de filas afectadas de forma asíncrona.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve una colección de resultados de tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Una colección de objetos del tipo especificado.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve un solo resultado o el valor predeterminado si no hay resultados.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Un objeto del tipo especificado o su valor predeterminado.</returns>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        /// <summary>
        /// Obtiene un IQueryable para usar en consultas LINQ que incluye filtro de status activo.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad para la consulta.</typeparam>
        /// <returns>IQueryable filtrado para estrategias LINQ.</returns>
        public IQueryable<T> GetActiveSet<T>() where T : class
        {
            var query = Set<T>().AsQueryable();

            // Filtramos por Status aplicando expresiones genéricas si la entidad tiene la propiedad Status
            var parameter = Expression.Parameter(typeof(T), "e");

            if (typeof(T).GetProperty("Status") != null)
            {
                try
                {
                    // Construimos una expresión lambda para filtrar por Status = true
                    var property = Expression.Property(parameter, "Status");
                    var value = Expression.Constant(true);
                    var equal = Expression.Equal(property, value);
                    var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                    // Aplicamos el filtro
                    query = query.Where(lambda);
                }
                catch
                {
                    // Si hay algún error, devolvemos el query sin filtrar
                }
            }

            return query;
        }

        /// <summary>
        /// Método auxiliar para obtener el valor de una propiedad de un objeto mediante reflexión.
        /// </summary>
        /// <param name="obj">Objeto del que se obtendrá el valor.</param>
        /// <param name="propertyName">Nombre de la propiedad.</param>
        /// <returns>Valor de la propiedad.</returns>
        private static bool GetPropertyValue(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return false;
            }
            return property.GetValue(obj, null) is bool value ? value : false;
        }

        /// <summary>
        /// Ejecuta una consulta con paginación utilizando LINQ.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="query">Consulta IQueryable base.</param>
        /// <param name="page">Número de página (comienza en 1).</param>
        /// <param name="pageSize">Tamaño de la página.</param>
        /// <returns>Colección paginada de elementos.</returns>
        public IQueryable<T> GetPaged<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Ejecuta una consulta LINQ y devuelve los resultados como una colección asíncrona.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="query">Consulta IQueryable a ejecutar.</param>
        /// <returns>Colección asíncrona de resultados.</returns>
        public async Task<List<T>> ToListAsyncSafe<T>(IQueryable<T> query)
        {
            if (query == null)
                return new List<T>();

            return await EntityFrameworkQueryableExtensions.ToListAsync(query);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity);

            var currentDateTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = currentDateTime;
                            entity.Status = true;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedAt = currentDateTime;
                            break;
                        case EntityState.Deleted:
                            // Convertimos el borrado en un borrado lógico
                            entry.State = EntityState.Modified;
                            entity.DeleteAt = currentDateTime;
                            entity.Status = false;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            /// <summary>
            /// Constructor del comando Dapper.
            /// </summary>
            /// <param name="context">Contexto de la base de datos.</param>
            /// <param name="text">Consulta SQL.</param>
            /// <param name="parameters">Parámetros opcionales.</param>
            /// <param name="timeout">Tiempo de espera opcional.</param>
            /// <param name="type">Tipo de comando SQL opcional.</param>
            /// <param name="ct">Token de cancelación.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            /// <summary>
            /// Define los parámetros del comando SQL.
            /// </summary>
            public CommandDefinition Definition { get; }

            /// <summary>
            /// Método para liberar los recursos.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}