using VideoGameCatalog.Api.Modules.Game.EfEntities;

namespace VideoGameCatalog.Api.Modules.Game
{
    // IGameDb defines the abstract contract for all DB operations needed in the Game module without exposing EF Core/DbContext details to callers.
    // It decouples the business layer from the concrete persistence technology (EF Core, SQL Server, etc.).
    // This design keeps the business logic independent from the persistence technology and makes the system easier to maintain, test, and evolve.
    
    // IGameDb lists the methods that any data‑access implementation must provide, such as loading games, saving games, deleting games, and retrieving lookup lists.
    // The interface itself contains no logic; it only describes what operations are available.

    // This interface is used inside the business logic layer.
    // The business logic class - GameBl (https://tinyurl.com/45hpwfpu) - receives an object implementing IGameDb through its constructor.

    // The constructor of GameBl declares its parameter as IGameDb.
    // This means that GameBl does not depend on any specific database implementation.
    // Instead, it depends on the abstraction.
    // The actual object passed into GameBl at runtime is an instance of GameDb, (https://tinyurl.com/6jabae3j) which is the only class in the project that implements IGameDb.

    // The reason the parameter is typed as IGameDb rather than GameDb is to allow the data‑access layer to be replaced without modifying the business logic.
    // If later you change how persistence works, you switch from GameDb to a another class which implements IGameDb - with minimal changes above it.
    // For example, one could create a new class such as GameDbPostgres, GameDbInMemory or GameTestMockDb and switch to it through dependency injection.
    
    // The API controller (https://tinyurl.com/38ankncc) does not use IGameDb directly; it communicates only with GameBl.
    // This is intentional: API controllers should not access the database layer.
    // They should call business logic, which then calls the data‑access abstraction.

    // Although the interface is used only in one place, it still serves its architectural purpose by keeping the business logic decoupled from the concrete database implementation.

    // ASYNC METHODS IN IGameDb

    // The methods in IGameDb all return Task or Task<T>.
    // This means every database operation is asynchronous.
    // Asynchronous methods allow the server to start a database query and then immediately free the thread to handle other requests while waiting for the database to respond.

    // WHY ASYNC EXISTS AT ALL

    // Database access is slow compared to CPU operations. When the API sends a SQL query, it must wait for the database to execute it and return the result.
    // Without async, the thread would block during this wait. In a web server, blocked threads reduce throughput and can cause the entire application to slow down under load.

    // Async solves this by allowing the thread to return to the thread pool while the database operation continues in the background.
    // When the result is ready, the Task completes and the continuation runs. This makes the API scalable and responsive.

    // WHAT Task AND Task<T> MEAN

    // Task represents an asynchronous operation that returns no value.  
    // Task<T> represents an asynchronous operation that returns a value of type T.

    // Examples:

    // * Task<List<GameDto>> means “an asynchronous operation that will eventually produce a list of GameDto objects”.
    // * Task means “an asynchronous operation that completes but does not return a value”, such as saving or deleting.

    // WHY WE NEED async IN THIS PROJECT

    // * Entity Framework Core uses async database APIs internally. Using async in IGameDb matches EF’s recommended usage.
    // * ASP.NET Core is designed around async. Synchronous database calls can cause thread starvation.
    // * The API may handle many requests at once. Async allows the server to scale without needing more threads.
    // * It prevents blocking the request pipeline, which improves performance under load.

    // In short, async and Task are used here because database operations are I/O‑bound, and asynchronous execution is the correct way to handle I/O in ASP.NET Core.
    
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
