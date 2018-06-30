using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Pjs1.Common.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

// Add-Migration CreateMigrationDatabasePostgre1 -Context Postgre1DbContext
// Update-Database CreateMigrationDatabasePostgre1 -Context Postgre1DbContext
// # NOTE
// < CreateMigrationDatabasePostgre1 > It's mean store in custom directory name
// < Postgre1DbContext > It's mean use this file target by class name

//Need to add this to IOC
//services.AddScoped(typeof(IEntityFrameworkRepository<,>), typeof(EntityFrameworkRepository<,>));
//services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


namespace Pjs1.Common.DAL
{
    public class Postgre1DbContext : DbContext
    {
        private readonly string _connectionString;
        public Postgre1DbContext(DbContextOptions<Postgre1DbContext> options)
            : base(options)
        {
            _connectionString = options.FindExtension<NpgsqlOptionsExtension>().ConnectionString;
        }

        // Add DbSet<OtherTable> here
        //public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Interlocutor> Interlocutor { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#if DEBUG
            var logEntitySql = new LoggerFactory();
            logEntitySql.AddProvider(new SqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(logEntitySql).UseNpgsql(_connectionString);
#else
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseNpgsql(_pgsql);
#endif

        }
    }
}
