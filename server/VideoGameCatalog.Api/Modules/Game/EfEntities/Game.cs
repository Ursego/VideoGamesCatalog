using VideoGameCatalog.Api.Util;

namespace VideoGameCatalog.Api.Modules.Game.EfEntities
{
    // Entity Framework (EF) is the data‑access technology used in this project to communicate with the database through .NET classes instead of writing SQL manually.
    // EF maps C# classes to database tables and maps class properties to table columns.
    // This allows the rest of the application to work with strongly typed objects while EF handles querying, inserting, updating, and deleting records.

    // EF entity classes represent the structure of database tables. Each entity corresponds to one table, and each property corresponds to one column.
    // These classes must follow certain requirements: they must be simple C# classes with public get/set properties, they must not contain business logic, and they must be compatible with EF conventions
    //     (for example, an Id property is treated as the primary key).
    
    // They also have limitations:
    // * They must be simple POCO classes with public properties.
    // * They should not contain behavior, business logic or validation rules.
    // * They should not depend on (be tightly coupled to) API models, DTOs, or services.
    // * They must match the database schema; otherwise EF will fail at runtime.
    // * Navigation properties must be virtual if lazy loading is desired.
    // * They should not contain constructors that require parameters, because EF needs to instantiate them automatically.
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int GameCategoryId { get; set; }
        public int AgeRatingId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImageUrl { get; set; }
        public DbBoolean IsActive { get; set; } = DbBoolean.Yes;
    }
}
