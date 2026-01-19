// *** GameBl ***
// Implements the Game module’s "B"usiness "L"ogic.
// Provides a single place for validations and domain/business behavior (now or in the future).
// Sits between the data layer (IGameDb) and the API layer (GameController), keeping them thin.
// Encapsulates mapping between EF entities and API DTOs, keeping DB schema concerns out of the API.
// Prevents data layer leakage: callers never need DbContext/EF entities; they work with DTOs only.

using VideoGameCatalog.Api.Modules.Game.Dtos;
using VideoGameCatalog.Api.Util;

namespace VideoGameCatalog.Api.Modules.Game
{
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
