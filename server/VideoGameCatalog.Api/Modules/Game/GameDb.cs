// *** GameDb ***
// Concrete EF Core implementation of IGameDb that executes queries/commands via GameCatalogDbContext.
// Centralizes all EF Core queries in one place, keeping GameBl focused on business rules and mapping.
// Keeps DB concerns out of higher layers (controller / business logic).
// Provides a single point to tune DB behavior.

using VideoGameCatalog.Api.Modules.Game.EfEntities;
using Microsoft.EntityFrameworkCore;

namespace VideoGameCatalog.Api.Modules.Game
{
    public class GameDb(GameCatalogDbContext db) : IGameDb
    {
        private readonly GameCatalogDbContext _db = db;

        public Task<List<EfEntities.Game>> SelectGameListAsync(string? name, CancellationToken cancellationToken = default)
        // If name provided, selects by it using the logic: WHERE Upper(NAME) LIKE Upper('%<provided name>%').
        // Otherwise, selects all games.
        {
            var query = _db.Games.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameUpper = name.Trim().ToUpperInvariant();
                var pattern = $"%{nameUpper}%";
                query = query.Where(g => EF.Functions.Like(g.Name.ToUpper(), pattern));
            }

            return query
                .OrderBy(g => g.Name)
                .ThenBy(g => g.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<EfEntities.Game> InsertGameAsync(EfEntities.Game game, CancellationToken cancellationToken = default)
        {
            _db.Games.Add(game);
            await _db.SaveChangesAsync(cancellationToken);
            return game;
        }

        public async Task<EfEntities.Game> UpdateGameAsync(EfEntities.Game game, CancellationToken cancellationToken = default)
        {
            var existing = await _db.Games.FirstOrDefaultAsync(g => g.Id == game.Id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Game not found. Id={game.Id}");
            existing.Name = game.Name;
            existing.Description = game.Description;
            existing.GameCategoryId = game.GameCategoryId;
            existing.AgeRatingId = game.AgeRatingId;
            existing.ReleaseDate = game.ReleaseDate;
            existing.CoverImageUrl = game.CoverImageUrl;
            existing.IsActive = game.IsActive;

            await _db.SaveChangesAsync(cancellationToken);
            return existing;
        }

        public async Task DeleteGameAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _db.Games.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            if (existing == null)
            {
                return;
            }

            _db.Games.Remove(existing);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public Task<List<GameCategory>> SelectGameCategoryListAsync(CancellationToken cancellationToken = default)
        {
            return _db.GameCategories
                .AsNoTracking()
                .OrderBy(c => c.Description)
                .ThenBy(c => c.Id)
                .ToListAsync(cancellationToken);
        }

        public Task<List<AgeRating>> SelectAgeRatingListAsync(CancellationToken cancellationToken = default)
        {
            return _db.AgeRatings
                .AsNoTracking()
                .OrderBy(r => r.Description)
                .ThenBy(r => r.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
