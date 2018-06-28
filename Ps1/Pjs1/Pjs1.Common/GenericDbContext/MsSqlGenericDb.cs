using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Pjs1.Common.DAL;

namespace Pjs1.Common.GenericDbContext
{
    public partial class MsSqlGenericDb : IdentityDbContext<GenericUser, GenericRole, int, GenericUserClaim , GenericUserRole , GenericUserLogin, GenericRoleClaim, GenericUserToken>
    {
        private readonly string _connectionString;
        public MsSqlGenericDb(DbContextOptions<MsSqlGenericDb> options) : base(options)
        {
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<GenericUser>()
            //    .HasOne(u => u.UserRoles) 
            //    .WithMany(u => u.)
            //    .Map(m =>
            //    {
            //        m.ToTable("UserRole");
            //        m.MapLeftKey("UserId");
            //        m.MapRightKey("RoleId");
            //    });



            //builder.Entity<GenericUser>(entity =>
            //{
            //    entity
            //        .HasOne(d => d.UserRole)
            //        .WithMany(p => p.Users)
            //        .HasForeignKey(d => d.Id)
            //        .OnDelete(DeleteBehavior.ClientSetNull); 
            //});


            //builder.Entity<GenericUserRole>(entity =>
            //{
            //    entity
            //        .HasMany(d => d.Roles)
            //        .WithOne(p => p.UserRole)
            //        .HasForeignKey(d => d.Id)
            //        .OnDelete(DeleteBehavior.ClientSetNull);
                 
            //});
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
