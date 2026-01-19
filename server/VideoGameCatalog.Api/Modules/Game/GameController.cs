// *** GameController ***
// The API layer.
// Defines HTTP endpoints for the Game module.
// Translates HTTP requests/responses to/from application calls (GameBl).
// Centralizes API surface: routes, verbs, and response codes are easy to review and document (Swagger).
// Standardizes validation/error responses: converts exceptions/results into 200/201/204/400/404, etc.

using VideoGameCatalog.Api.Modules.Game.Dtos;
using VideoGameCatalog.Api.Util;
using Microsoft.AspNetCore.Mvc;

namespace VideoGameCatalog.Api.Modules.Game
{
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
