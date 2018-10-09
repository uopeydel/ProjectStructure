using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;

namespace Pjs1.Common.GenericDbContext
{
    public interface IMsSqlGenericDb { }
    public partial class MsSqlGenericDb : IdentityDbContext<GenericUser, GenericRole, int, GenericUserClaim, GenericUserRole, GenericUserLogin, GenericRoleClaim, GenericUserToken> , IMsSqlGenericDb
    {
        private readonly string _connectionString;
        public MsSqlGenericDb(IServiceProvider serviceProvider/*,DbContextOptions<MsSqlGenericDb> options*/)  
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<MsSqlGenericDb>>();
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
        }

        // Add DbSet<OtherTable> here
        public virtual DbSet<GenericUser> User { get; set; }
        public virtual DbSet<GenericRole> Role { get; set; }

        public virtual DbSet<GenericUserClaim> UserClaim { get; set; }
        public virtual DbSet<GenericUserRole> UserRole { get; set; }
        public virtual DbSet<GenericUserLogin> UserLogin { get; set; }
        public virtual DbSet<GenericRoleClaim> RoleClaim { get; set; }
        public virtual DbSet<GenericUserToken> UserToken { get; set; }

        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Interlocutor> Interlocutor { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region MapManyToMany Contact


            builder.Entity<Contact>(entity =>
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

            builder.Entity<Conversation>(entity =>
            {
                entity
                    .HasOne(ho => ho.ConversationReceiver)
                    .WithMany(wm => wm.ConversationReceiver)
                    .HasForeignKey(hfk => hfk.ConversationReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity
                    .HasOne(ho => ho.ConversationSender)
                    .WithMany(wm => wm.ConversationSender)
                    .HasForeignKey(hfk => hfk.ConversationSenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });


            #endregion

            #region OneToOne 

            builder.Entity<GenericUser>()
                .HasOne(ho => ho.Interlocutor)
                .WithOne(wo => wo.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Interlocutor>()
                .HasOne(ho => ho.User)
                .WithOne(wo => wo.Interlocutor)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
