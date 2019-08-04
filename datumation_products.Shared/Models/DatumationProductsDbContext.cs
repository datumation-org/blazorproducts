using Microsoft.EntityFrameworkCore;

namespace datumation_products.Shared.Models {
    public class DatumationProductsDbContext : DbContext {
        public DatumationProductsDbContext (DbContextOptions<DatumationProductsDbContext> options) : base (options) { }
        // public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        // public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        // public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<UserOrderDetails> OrderDetails { get; set; }
        // public virtual DbSet<Orders> Orders { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        //public virtual DbSet<Result> ProviderData { get; set; }
        //public virtual DbSet<ResultWithCombo> ResultWithCombo { get; set; }

        //public virtual DbSet<ProviderTypesByState> ProviderTypesByState { get; set; }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                //   optionsBuilder.UseSqlite ($"{ConfigurationFactory.Instance.Configuration().DefaultConnection}");
            }
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            // modelBuilder.Entity<AspNetRoles>(entity =>
            // {
            //     entity.HasIndex(e => e.Name)
            //         .HasName("RoleNameIndex")
            //         .IsUnique();

            //     entity.Property(e => e.Id)
            //         .HasMaxLength(128)
            //         .ValueGeneratedNever();

            //     entity.Property(e => e.Name)
            //         .IsRequired()
            //         .HasMaxLength(256);
            // });

            modelBuilder.Entity<AspNetUserClaims> (entity => {
                entity
                    .
                HasIndex (e => e.UserId);

                entity.Property (e => e.UserId)
                    .IsRequired ()
                    .HasMaxLength (128);

                entity.HasOne (d => d.User)
                    .WithMany (p => p.AspNetUserClaims)
                    .HasForeignKey (d => d.UserId);
            });
            //modelBuilder.Entity<Result>(entity =>
            //{
            //    entity.HasKey(e => e.NPI);
            //});

            modelBuilder.Entity<AspNetUserLogins> (entity => {
                entity.HasKey (e => new { e.LoginProvider, e.ProviderKey, e.UserId });

                entity.HasIndex (e => e.UserId);

                entity.Property (e => e.LoginProvider).HasMaxLength (128);

                entity.Property (e => e.ProviderKey).HasMaxLength (128);

                entity.Property (e => e.UserId).HasMaxLength (128);

                entity.HasOne (d => d.User)
                    .WithMany (p => p.AspNetUserLogins)
                    .HasForeignKey (d => d.UserId);
            });

            // modelBuilder.Entity<AspNetUserRoles>(entity =>
            // {
            //     entity.HasKey(e => new { e.UserId, e.RoleId });

            //     entity.HasIndex(e => e.RoleId)
            //         .HasName("IX_RoleId");

            //     entity.HasIndex(e => e.UserId)
            //         .HasName("IX_UserId");

            //     entity.Property(e => e.UserId).HasMaxLength(128);

            //     entity.Property(e => e.RoleId).HasMaxLength(128);

            //     entity.HasOne(d => d.Role)
            //         .WithMany(p => p.AspNetUserRoles)
            //         .HasForeignKey(d => d.RoleId)
            //         .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

            //     entity.HasOne(d => d.User)
            //         .WithMany(p => p.AspNetUserRoles)
            //         .HasForeignKey(d => d.UserId)
            //         .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            // });

            modelBuilder.Entity<AspNetUsers> (entity => {
                entity.HasIndex (e => e.UserName)

                    .IsUnique ();

                entity.Property (e => e.Id)
                    .HasMaxLength (128)
                    .ValueGeneratedNever ();

                entity.Property (e => e.Email).HasMaxLength (256);

                entity.Property (e => e.LockoutEndDateUtc);

                entity.Property (e => e.UserName)
                    .IsRequired ()
                    .HasMaxLength (256);
            });

            modelBuilder.Entity<Carts> (entity => {
                entity.HasIndex (e => e.ItemId);

                entity.Property (e => e.Id);

                entity.Property (e => e.DateCreated);

                entity.HasOne (d => d.Item)
                    .WithMany (p => p.Carts)
                    .HasForeignKey (d => d.ItemId);
            });

            modelBuilder.Entity<Categories> (entity => {
                entity.Property (e => e.Id);
            });

            modelBuilder.Entity<Items> (entity => {
                // entity.ToTable("ItemProducts");
                entity.HasIndex (e => e.CategoryId);
                entity.Property (e => e.Id);

                entity.Property (e => e.ItemPictureUrl).HasMaxLength (1024);
                entity.Property (e => e.ColorScheme);
                entity.Property (e => e.Name)
                    .IsRequired ()
                    .HasMaxLength (160);

                entity.HasOne (d => d.Category)
                    .WithMany (p => p.Items)
                    .HasForeignKey (d => d.CategoryId);
            });

            // modelBuilder.Entity<MigrationHistory>(entity =>
            // {
            //     entity.HasKey(e => new { e.MigrationId, e.ContextKey });

            //     entity.ToTable("__MigrationHistory");

            //     entity.Property(e => e.MigrationId).HasMaxLength(150);

            //     entity.Property(e => e.ContextKey).HasMaxLength(300);

            //     entity.Property(e => e.Model).IsRequired();

            //     entity.Property(e => e.ProductVersion)
            //         .IsRequired()
            //         .HasMaxLength(32);
            // });

            // modelBuilder.Entity<OrderDetails>(entity =>
            // {
            //     entity.HasKey(e => e.OrderDetailId);

            //     entity.HasIndex(e => e.ItemId)
            //         .HasName("IX_ItemId");

            //     entity.HasIndex(e => e.OrderId)
            //         .HasName("IX_OrderId");

            //     entity.HasOne(d => d.Item)
            //         .WithMany(p => p.OrderDetails)
            //         .HasForeignKey(d => d.ItemId)
            //         .HasConstraintName("FK_dbo.OrderDetails_dbo.Items_ItemId");

            //     entity.HasOne(d => d.Order)
            //         .WithMany(p => p.OrderDetails)
            //         .HasForeignKey(d => d.OrderId)
            //         .HasConstraintName("FK_dbo.OrderDetails_dbo.Orders_OrderId");
            // });
            //modelBuilder.Entity<ProviderTypesByState>(entity =>
            //{
            //    entity.HasKey("NPI");
            //});

            //modelBuilder.Entity<ResultWithCombo>(entity =>
            //{
            //    entity.HasKey("NPI");
            //});

            modelBuilder.Entity<Orders> (entity => {
                entity.HasKey (e => e.OrderId);

                entity.Property (e => e.Address)
                    .IsRequired ()
                    .HasMaxLength (70);

                entity.Property (e => e.City)
                    .IsRequired ()
                    .HasMaxLength (40);

                entity.Property (e => e.Country)
                    .IsRequired ()
                    .HasMaxLength (40);

                entity.Property (e => e.Experation);

                entity.Property (e => e.FirstName)
                    .IsRequired ()
                    .HasMaxLength (160);

                entity.Property (e => e.LastName)
                    .IsRequired ()
                    .HasMaxLength (160);

                entity.Property (e => e.OrderDate);

                entity.Property (e => e.Phone)
                    .IsRequired ()
                    .HasMaxLength (24);

                entity.Property (e => e.PostalCode)
                    .IsRequired ()
                    .HasMaxLength (10);

                entity.Property (e => e.State)
                    .IsRequired ()
                    .HasMaxLength (40);
            });
        }
    }
}