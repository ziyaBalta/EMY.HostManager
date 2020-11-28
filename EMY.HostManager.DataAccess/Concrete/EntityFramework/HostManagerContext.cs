using EMY.HostManager.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMY.HostManager.DataAccess.Concrete.EntityFramework
{
    public class HostManagerContext : DbContext
    {
        public string ConnectionStringProp { private get; set; }
        string LocalSqlServer = "Server=.;Database=dbHostManager;User Id=SA;Password=0545696s;";
        string LocalSqlite = "Data Source=C://dbHostManager.db";
        DatabaseSystem dbSysem;

        #region DebugModeSettings
        /// <summary>
        /// Sqlite local database ( FOR TESTS )
        /// </summary>
        public HostManagerContext()
        {
            ConnectionStringProp = LocalSqlite;
            dbSysem = DatabaseSystem.Sqlite;
        }

        /// <summary>
        /// Sqlserver or Sqlite local connections ( FOR TESTS )
        /// </summary>
        /// <param name="system"></param>
        public HostManagerContext(DatabaseSystem system)
        {
            switch (system)
            {
                case DatabaseSystem.SqlServer:
                    ConnectionStringProp = LocalSqlServer;
                    break;
                case DatabaseSystem.Sqlite:
                    ConnectionStringProp = LocalSqlite;

                    break;
                default:
                    break;
            }
            dbSysem = system;
        }

        #endregion


        #region ReleaseModeSettings
        /// <summary>
        /// Release connection
        /// </summary>
        /// <param name="system">Database system</param>
        /// <param name="ConnectionString">Connection string</param>
        public HostManagerContext(DatabaseSystem system, string ConnectionString)
        {
            dbSysem = system;
            ConnectionStringProp = ConnectionString;
        }
        #endregion


        #region DbSets
        public DbSet<ServerInformation> ServerInformations { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateParameter> TemplateParameters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (dbSysem)
            {
                case DatabaseSystem.SqlServer:
                    optionsBuilder.UseSqlServer(ConnectionStringProp, builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                    });
                    base.OnConfiguring(optionsBuilder);
                    break;
                case DatabaseSystem.Sqlite:
                    optionsBuilder.UseSqlite(ConnectionStringProp);
                    break;
                default:
                    break;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }

        #region SaveChangesOverrides
        public override int SaveChanges()
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added || EntityState.Deleted == entry.State
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    if (Debugger.IsAttached)
                    {
                        Debug.Indent();
                        foreach (var valid in validationResults)
                        {
                            Debug.WriteLine(string.Join(",", valid.MemberNames) + " is not validated! Error:" + valid.ErrorMessage);
                        }

                        Debug.Unindent();
                    }
                }
            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added || EntityState.Deleted == entry.State
                            select entry.Entity);

            var validationResults = new List<ValidationResult>();
            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    if (Debugger.IsAttached)
                    {
                        Debug.Indent();
                        foreach (var valid in validationResults)
                        {
                            Debug.WriteLine(string.Join(",", valid.MemberNames) + " is not validated! Error:" + valid.ErrorMessage);
                        }

                        Debug.Unindent();
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion
        public enum DatabaseSystem
        {
            SqlServer,
            Sqlite
        }
    }
}
