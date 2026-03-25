
using VideoGameCatalog.Api.Modules.Game.EfEntities;
using Microsoft.EntityFrameworkCore;

namespace VideoGameCatalog.Api.Modules.Game
{
    // GameDb is a concrete implementation of the IGameDb interface (https://tinyurl.com/5n7wy4v7).
    // While IGameDb defines *what* operations the data layer must provide, GameDb defines *how* those operations are actually performed.
    
    // GameDb communicates directly with the real Oracle database using Entity Framework Core and executes queries/commands via GameCatalogDbContext (https://tinyurl.com/mrxfxrcx).
    // Every method inside GameDb executes real SQL queries through the EF DbContext, loads entities, maps them to DTOs, and returns the results to the business logic layer.

    // GameDb provides a single point to tune DB behavior by isolating all database‑specific code in one place.
    // This keeps the rest of the application independent from the persistence technology and allows GameBl to focus only on business rules and mapping.
    // Business logic does not know anything about Oracle, EF Core, or SQL; it only calls the methods defined in IGameDb.
    // Because of this separation, GameDb can be replaced or retired in the future without changing the business logic.

    // GameDb also contains the actual mapping between EF entities and DTOs.
    // EF entities represent the database tables, while DTOs represent the API contract.
    // GameDb is the correct place to convert between these two models, because it sits at the boundary between persistence and business logic.

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
