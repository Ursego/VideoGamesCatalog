using VideoGameCatalog.Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VideoGameCatalog.Api.Modules.Game.EfEntities
{
    public class GameCatalogDbContext(DbContextOptions<GameCatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameCategory> GameCategories => Set<GameCategory>();
        public DbSet<AgeRating> AgeRatings => Set<AgeRating>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Stores DbBoolean as 'Y'/'N' in the DB and reads it back to enum.
            var dbBooleanConverter = new ValueConverter<DbBoolean, string>(
                v => v == DbBoolean.Yes ? "Y" : "N",
                v => v == "Y" ? DbBoolean.Yes : DbBoolean.No
            );

            modelBuilder.Entity<GameCategory>(e =>
            {
                e.ToTable("GameCategory");

                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Description)
                    .HasMaxLength(200)
                    .IsRequired();
            });

            modelBuilder.Entity<AgeRating>(e =>
            {
                e.ToTable("AgeRating");

                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Description)
                    .HasMaxLength(200)
                    .IsRequired();
            });

            modelBuilder.Entity<Game>(e =>
            {
                e.ToTable("Game", t => t.HasCheckConstraint("CK_Game_IsActive", "IsActive IN ('Y','N')"));

                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(x => x.Description)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false);

                e.Property(x => x.GameCategoryId)
                    .IsRequired();

                e.Property(x => x.AgeRatingId)
                    .IsRequired();

                e.Property(x => x.ReleaseDate)
                    .HasColumnType("date")
                    .IsRequired(false);

                e.Property(x => x.CoverImageUrl)
                    .HasMaxLength(500)
                    .IsRequired(false);

                e.Property(x => x.IsActive)
                    .HasColumnType("char(1)")
                    .HasConversion(dbBooleanConverter)
                    .HasDefaultValue(DbBoolean.Yes)
                    .IsRequired();
            });
        }
    }
}