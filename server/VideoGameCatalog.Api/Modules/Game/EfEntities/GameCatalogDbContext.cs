using VideoGameCatalog.Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VideoGameCatalog.Api.Modules.Game.EfEntities
{
    // GameCatalogDbContext is the class that represents the Oracle database inside the application.
    // It is the single place where EF Core learns how the database is structured.
    // All data access in the Game module goes through this DbContext.
    
    // Our DbContext contains three tables, and each table is represented by one entity class: Game, GameCategory, and AgeRating. 
    // These three classes describe the structure of the data stored in the database, and the DbSet properties inside the DbContext expose them to the rest of the application.
    
    // When the application loads games, categories, or age ratings, it does so through these DbSet properties.
    // When it saves changes, EF Core uses the configuration defined in OnModelCreating to generate the correct SQL for Oracle.

    // In summary, GameCatalogDbContext defines the three tables used by the Game module, exposes them through DbSet properties, and configures their mapping to the Oracle database.
    // It is the central point where the object model of the application meets the relational model of the database.
    public class GameCatalogDbContext(DbContextOptions<GameCatalogDbContext> options) : DbContext(options)
    {
        // A DbSet is EF Core’s way of saying “this is a table.” For example, DbSet<Game> Games means that the Game class corresponds to a table in the database that stores games.
        // It is a set of rows, where each row is represented as an EF entity object. So, read it as "dataset".
        // Through these DbSet objects, the application can query the tables, insert new rows, update existing rows, and delete rows.
        // EF Core automatically translates these operations into SQL commands that the DB understands.
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameCategory> GameCategories => Set<GameCategory>();
        public DbSet<AgeRating> AgeRatings => Set<AgeRating>();

        // The OnModelCreating method receives a ModelBuilder object, which is used to configure how each entity maps to its table.
        // Even though EF Core can guess many things automatically, it is common to configure the mapping explicitly.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Stores DbBoolean as 'Y'/'N' in the DB and reads it back to enum.
            var dbBooleanConverter = new ValueConverter<bool, DbBoolean>(
                boolVal => boolVal ? DbBoolean.Yes : DbBoolean.No,
                dbVal => dbVal == DbBoolean.Yes
            );

            // Inside OnModelCreating, the code calls modelBuilder.Entity<[EF entity class]>(e => { ... }). This tells EF Core: “configure the [EF entity class] entity.”
            // The arrow function (the lambda) receives a builder object that allows the code to specify the table name, the primary key, the column names, and any relationships.
            // Inside the braces, you can configure everything EF Core needs to know about how this class corresponds to the database table.
            modelBuilder.Entity<GameCategory>(e =>
            {
                e.ToTable("GameCategory"); // means: “for the GameCategory entity, map it to the GameCategory table.”

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
