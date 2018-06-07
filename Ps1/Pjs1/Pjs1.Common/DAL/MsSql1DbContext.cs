using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Pjs1.Common.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

// Add-Migration CreateMigrationDatabaseMsSql1 -Context MsSql1DbContext
// Update-Database CreateMigrationDatabaseMsSql1 -Context MsSql1DbContext -Verbose
// # NOTE
// < CreateMigrationDatabaseMsSql1 > It's mean store in custom directory name
// < MsSql1DbContext > It's mean use this file target by class name

//Need to add this to IOC
//services.AddScoped(typeof(IEntityFrameworkRepository<,>), typeof(EntityFrameworkRepository<,>));
//services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


namespace Pjs1.Common.DAL
{
    public class MsSql1DbContext : DbContext
    {
        private readonly string _connectionString;
        public MsSql1DbContext(DbContextOptions<MsSql1DbContext> options)
            : base(options)
        {
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
        }

        // Add DbSet<OtherTable> here
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Interlocutor> Interlocutor { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region MapManyToMany Contact


            modelBuilder.Entity<Contact>(entity =>
            {
                entity
                .HasOne(d => d.ContactReceiver)
                .WithMany(p => p.ContactReceiver)
                .HasForeignKey(d => d.ContactReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity
                .HasOne(d => d.ContactSender)
                .WithMany(p => p.ContactSender)
                .HasForeignKey(d => d.ContactSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            #endregion

            #region MapManyToMany Conversation

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity
                .HasOne(pt => pt.ConversationReceiver)
                .WithMany(p => p.ConversationReceiver)
                .HasForeignKey(pt => pt.ConversationReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity
                .HasOne(pt => pt.ConversationSender)
                .WithMany(p => p.ConversationSender)
                .HasForeignKey(pt => pt.ConversationSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            });


            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#if DEBUG
            var logEntitySql = new LoggerFactory();
            logEntitySql.AddProvider(new SqlLoggerProvider());
            optionsBuilder.UseLoggerFactory(logEntitySql).UseSqlServer(_connectionString);
#else
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseSqlServer(_pgsql);
#endif

        }
    }
}
