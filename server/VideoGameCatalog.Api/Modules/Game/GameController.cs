using VideoGameCatalog.Api.Modules.Game.Dtos;
using VideoGameCatalog.Api.Util;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameCatalog.Api.Modules.Game
{
    // GameController is the entry point for all HTTP requests related to games.
    // It belongs to the API layer and exposes endpoints that clients (such as a web frontend) can call.
    // Each public method of GameController corresponds to an HTTP route, such as loading a list of games, loading a single game, saving a game, deleting a game, or retrieving dropdown data.
    
    // GameController:
    // * Centralizes API surface: routes, verbs, and response codes are easy to review and document.
    // * Standardizes validation/error responses: converts exceptions/results into 200/201/204/400/404, etc.

    // The controller does not contain business logic and does not talk to the database directly.
    // It only translates HTTP requests/responses to GameBl calls and back.
    // This separation keeps the controller thin and focused on handling HTTP concerns: routing, request/response formatting, and returning appropriate status codes.

    // When a request arrives, the API controller forwards it to GameBl.
    // GameBl then performs any necessary logic and calls the data‑access layer - GameDb (https://tinyurl.com/6jabae3j) through IGameDb (https://tinyurl.com/5n7wy4v7) - to retrieve or modify data.
    // The API controller simply receives requests from the client and returns the results to the client.

    // This design follows standard ASP.NET Core architecture:  
    // * Controllers handle HTTP.  
    // * Business logic handles rules and workflows.  
    // * Data‑access classes handle persistence.

    // GameController is intentionally simple.
    // It acts as a bridge between the outside world and the internal application layers, ensuring that the API remains clean, maintainable, and easy to extend.
                                                                                   
    [ApiController]
    public class GameController(GameBl bl) : ControllerBase
    {
        private readonly GameBl _bl = bl;

        [HttpGet]
        [Route("games")]
        public Task<List<GameDto>> SelectGameList([FromQuery] string? name, CancellationToken cancellationToken)
        {
            return _bl.SelectGameListAsync(name, cancellationToken);
        }
        
        [HttpPost]
        [Route("games")]
        public async Task<ActionResult<GameDto>> InsertGame([FromBody] GameDto dto, CancellationToken cancellationToken)
        {
            var saved = await _bl.InsertGameAsync(dto, cancellationToken);
            return Created($"/games/{saved.Id}", saved);
        }

        [HttpPut]
        [Route("games/{id:int}")]
        public async Task<ActionResult<GameDto>> UpdateGame(int id, [FromBody] GameDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var saved = await _bl.UpdateGameAsync(id, dto, cancellationToken);
                return Ok(saved);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("games/{id:int}")]
        public async Task<IActionResult> DeleteGame(int id, CancellationToken cancellationToken)
        {
            await _bl.DeleteGameAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [Route("gamecategories")]
        public Task<List<DropdownEntryDto>> SelectGameCategoryList(CancellationToken cancellationToken)
        {
            return _bl.SelectGameCategoryListAsync(cancellationToken);
        }

        [HttpGet]
        [Route("ageratings")]
        public Task<List<DropdownEntryDto>> SelectAgeRatingList(CancellationToken cancellationToken)
        {
            return _bl.SelectAgeRatingListAsync(cancellationToken);
        }
    }
}
