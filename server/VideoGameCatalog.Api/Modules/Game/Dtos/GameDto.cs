using VideoGameCatalog.Api.Util;

namespace VideoGameCatalog.Api.Modules.Game.Dtos
{
    // DTO is a simple data‑carrier class used only for outside world API, i.e. communication between the middle tier and front end.

    // Why do we need DTOs while EF entity classes already exist?
    // Because EF entities represent the persistence model, not the API contract. The API must not expose EF entity classes directly to clients. These two concerns must stay separate:
    // * EF entities represent the internal database structure. They contain the DB table fields.
    // * DTOs represent the data shape that the API sends and receives. They contain the fields that the client needs and can include fields which don't exist in the DB.

    // More details:
    // * EF entities are tied to the database schema and may contain navigation properties, foreign keys, or internal fields that should not be exposed publicly.
    // * DTOs protect the API from accidental schema leaks. If the database changes, the API contract can remain stable by adjusting the mapping between entities and DTOs.
    // * DTOs prevent over‑posting and under‑posting attacks. Clients can only send fields that the DTO allows, not arbitrary EF entity properties.
    // * DTOs allow the API to evolve independently. You can add computed fields, rename fields, or hide internal fields without touching the database.
    // * DTOs avoid exposing lazy‑loading proxies or EF‑specific behavior to clients, which could cause serialization issues.
    // * DTOs keep the domain clean. EF entities stay focused on persistence, while DTOs focus on communication.

    // DTOs are intentionally simple, containing only plain properties with no logic. This makes it safe to serialize, safe to expose, and easy to version over time.
    
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int GameCategoryId { get; set; }
        public int AgeRatingId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? CoverImageUrl { get; set; }
        public DbBoolean IsActive { get; set; }
    }
} 
