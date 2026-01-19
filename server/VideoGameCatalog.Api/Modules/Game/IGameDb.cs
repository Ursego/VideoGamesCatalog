// *** IGameDb ***
// Defines the contract for all DB operations needed by the Game module without exposing EF Core/DbContext details to callers.
// Decouples the business layer from the concrete persistence technology (EF Core, SQL Server, etc.).
// If later you change how persistence works (e.g., different DBMS, stored procedures, caching layer),
//      you can swap GameDb behind the interface with minimal changes above it.
// Enables fast unit testing of GameBl by injecting a fake/mock IGameDb (no real DB required).

using VideoGameCatalog.Api.Modules.Game.EfEntities;

namespace VideoGameCatalog.Api.Modules.Game
{
    public interface IGameDb
    {
        Task<List<EfEntities.Game>> SelectGameListAsync(string? name, CancellationToken cancellationToken = default);

        Task<EfEntities.Game> InsertGameAsync(EfEntities.Game game, CancellationToken cancellationToken = default);

        Task<EfEntities.Game> UpdateGameAsync(EfEntities.Game game, CancellationToken cancellationToken = default);

        Task DeleteGameAsync(int id, CancellationToken cancellationToken = default);

        Task<List<GameCategory>> SelectGameCategoryListAsync(CancellationToken cancellationToken = default);

        Task<List<AgeRating>> SelectAgeRatingListAsync(CancellationToken cancellationToken = default);
    }
}
// in IGameDb
