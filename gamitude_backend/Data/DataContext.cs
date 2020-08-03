using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using gamitude_backend.Model;
using Microsoft.AspNetCore.Identity;
using gamitude_backend.Extensions;

namespace gamitude_backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<UserToken> userTokens { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set timeCreated / timeUpdated appropriately
                if (entry.Entity is IBaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.timeUpdated = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("timeCreated").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.timeCreated = utcNow; 
                            trackable.timeUpdated = utcNow;
                            break;
                    }
                }
            }
        }


        //TODO add constrains on users
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("bill_gent");
            base.OnModelCreating(modelBuilder);

            // //MANY TO MANY 
            // //FOR RESOURCE RESOURCE_FIELD 
            // modelBuilder.Entity<ResourceLinkResourceField>()
            //     .HasKey(o => new { o.resourceId, o.resourceFieldId });

            // modelBuilder.Entity<ResourceLinkResourceField>()
            //     .HasOne<Resource>(rlrf => rlrf.resource)
            //     .WithMany(r => r.resourceFields)
            //     .HasForeignKey(rlrf => rlrf.resourceId);

            // modelBuilder.Entity<ResourceLinkResourceField>()
            //     .HasOne<ResourceField>(rlrf => rlrf.resourceField)
            //     .WithMany(rf => rf.resources)
            //     .HasForeignKey(rlrf => rlrf.resourceFieldId);




            //SET ALL TABLENAMES AND COLUMN NAMES TO SNAKE CASE FOR POSTGRESS (COLUMN ANNOTATION NOT NECCESARY)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().camelToSnakeCase());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().camelToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().camelToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().camelToSnakeCase());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().camelToSnakeCase());
                }
            }

            //SEED DATA
            //--Users
            modelBuilder.Entity<User>()
                .HasData(
                    new User { Id = "9d95139f-ba68-4735-8e7e-ce53f0f49792", UserName = "admin", NormalizedUserName = "ADMIN", Email = "admin@gamitude.rocks", NormalizedEmail = "ADMIN@GAMITUDE.ROCKS", PasswordHash = "AQAAAAEAACcQAAAAEENnk49V/14/OGX7ZctDx0BuMBHVCE+ShxUMElnQkL8LjT6brc8zE/l/zY36VRf6gg==", SecurityStamp = "5Y2ZI6YO2W2EUIDKOF7VBVN5MSW34PAN", ConcurrencyStamp = "513c7921-0a66-47ef-9eb7-8ecf4e1ad2fc", PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEnabled = true, AccessFailedCount = 0, EmailConfirmed = false }
                );

        }
    }
}