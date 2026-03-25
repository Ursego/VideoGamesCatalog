using VideoGameCatalog.Api.Modules.Game.Dtos;
using VideoGameCatalog.Api.Util;

namespace VideoGameCatalog.Api.Modules.Game
{
    // GameBl is the "B"usiness "L"ogic layer for the Game module.
    // Provides a single place for validations and domain/business behavior (now or in the future).
    
    // Its job is to sit between the API layer (GameController - https://tinyurl.com/38ankncc) and the database layer (IGameDb - https://tinyurl.com/5n7wy4v7), keeping them thin.
    // The API controller should never talk to the database directly, and the database layer should not contain business logic.
    // GameBl is the place where these concerns meet.
    // It encapsulates mapping between EF entities and API DTOs, keeping DB schema concerns out of the API.
    // Prevents data layer leakage: callers never need DbContext/EF entities; they work with DTOs only.

    // The class receives an IGameDb instance through its constructor.
    // This means GameBl does not depend on the concrete GameDb class. Instead, it depends on the abstraction.
    // This allows the data‑access implementation to be replaced in the future without changing the business logic.

    // WHAT ELSE COULD BE IN A BUSINESS‑LOGIC CLASS LIKE THIS

    // Inside GameBl, each public method corresponds to an operation the API exposes: loading a list of games, loading a single game, saving a game, deleting a game, and loading dropdown lists.
    // In our minimal example, GameBl simply forwards these calls to the data layer.
    // But this is also the place where additional logic could be added, such as validation, caching, permission checks, or combining data from multiple sources.
    // In a real‑world application, a BL class often contains more responsibilities. For example:

    // * Validation of incoming data before saving it (checking required fields, checking formats, enforcing rules).
    // * Permission checks (verifying whether the current user is allowed to perform the operation).
    // * Combining data from multiple repositories or services.
    // * Applying business rules (for example, preventing deletion of active items, or enforcing category constraints).
    // * Logging or auditing actions.
    // * Caching frequently accessed data.
    // * Transforming or enriching data before returning it to the controller.
    // * Handling domain events or notifications.
    
    public class GameBl(IGameDb db)
    {
        private readonly IGameDb _db = db;

        public async Task<List<GameDto>> SelectGameListAsync(string? name, CancellationToken cancellationToken = default)
        {
            var list = await _db.SelectGameListAsync(name, cancellationToken);
            return list.Select(MapToDto).ToList();
        }

        public async Task<GameDto> InsertGameAsync(GameDto game, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(game);

            var entity = MapToEntity(game);
            entity.Id = 0;

            var saved = await _db.InsertGameAsync(entity, cancellationToken);
            return MapToDto(saved);
        }

        public async Task<GameDto> UpdateGameAsync(int id, GameDto game, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(game);

            if (game.Id != 0 && game.Id != id)
            {
                throw new ArgumentException($"Body Id must match route id. Route id={id}, Body Id={game.Id}", nameof(game));
            }

            var entity = MapToEntity(game);
            entity.Id = id;

            var saved = await _db.UpdateGameAsync(entity, cancellationToken);
            return MapToDto(saved);
        }

        public Task DeleteGameAsync(int id, CancellationToken cancellationToken = default)
        {
            return _db.DeleteGameAsync(id, cancellationToken);
        }

        public async Task<List<DropdownEntryDto>> SelectGameCategoryListAsync(CancellationToken cancellationToken = default)
        {
            var list = await _db.SelectGameCategoryListAsync(cancellationToken);
            return list.Select(x => new DropdownEntryDto(x.Id, x.Description)).ToList();
        }

        public async Task<List<DropdownEntryDto>> SelectAgeRatingListAsync(CancellationToken cancellationToken = default)
        {
            var list = await _db.SelectAgeRatingListAsync(cancellationToken);
            return list.Select(x => new DropdownEntryDto(x.Id, x.Description)).ToList();
        }

        private static GameDto MapToDto(EfEntities.Game entity)
        {
            return new GameDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                GameCategoryId = entity.GameCategoryId,
                AgeRatingId = entity.AgeRatingId,
                ReleaseDate = entity.ReleaseDate,
                CoverImageUrl = entity.CoverImageUrl,
                IsActive = entity.IsActive
            };
        }

        private static EfEntities.Game MapToEntity(GameDto game)
        {
            return new EfEntities.Game
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                GameCategoryId = game.GameCategoryId,
                AgeRatingId = game.AgeRatingId,
                ReleaseDate = game.ReleaseDate,
                CoverImageUrl = game.CoverImageUrl,
                IsActive = game.IsActive
            };
        }
    }
}
