using VideoGameCatalog.Api.Util;

namespace VideoGameCatalog.Api.Modules.Game.EfEntities
{
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