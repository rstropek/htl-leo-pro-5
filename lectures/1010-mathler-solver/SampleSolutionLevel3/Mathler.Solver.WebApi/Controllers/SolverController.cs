using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mathler.Solver.WebApi.Controllers
{
    [Route("api/[controller]/game")]
    [ApiController]
    public class SolverController : ControllerBase
    {
        private readonly MathlerContext context;
        private readonly IPuzzleSolverFactory factory;

        public record NewGameRequestDto(int ExpectedResult);

        public record GameIdDto(int GameId);

        public record StoreResultDto(string Formula, string Result);

        public record GuessResultDto(string Formula);

        public SolverController(MathlerContext context, IPuzzleSolverFactory factory)
        {
            this.context = context;
            this.factory = factory;
        }

        /// <summary>
        /// Creates a new game with a given expected result
        /// </summary>
        /// <response code="201">Game created</response>
        [HttpPost(Name = nameof(NewGame))]
        public async Task<ActionResult<GameIdDto>> NewGame(NewGameRequestDto req)
        {
            var newGame = new Game() { ExpectedResult = req.ExpectedResult };
            context.Games.Add(newGame);
            await context.SaveChangesAsync();

            return Created(string.Empty, new GameIdDto(newGame.Id));
        }

        /// <summary>
        /// Stores a guess for an existing game
        /// </summary>
        /// <response code="201">Result Stored</response>
        /// <response code="404">Game not found</response>
        /// <response code="400">Error in formula or result</response>
        [HttpPost("{id}", Name = nameof(StoreResult))]
        public async Task<IActionResult> StoreResult(int id, StoreResultDto req)
        {
            var game = await context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var solver = factory.Create(game.ExpectedResult);
            try
            {
                solver.StoreResult(req.Formula, req.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

            context.Guesses.Add(new()
            {
                GameId = id,
                Formula = req.Formula,
                Result = req.Result
            });

            await context.SaveChangesAsync();

            return Created(string.Empty, null);
        }

        /// <summary>
        /// Gets a guess for an existing game
        /// </summary>
        /// <response code="404">Game not found</response>
        [HttpGet("{id}", Name = nameof(Guess))]
        public async Task<ActionResult<GuessResultDto>> Guess(int id)
        {
            var game = await context.Games.Include(g => g.Guesses)
                .AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var solver = factory.Create(game.ExpectedResult);
            foreach(var guess in game.Guesses.OrderBy(g => g.Id))
            {
                solver.StoreResult(guess.Formula, guess.Result);
            }

            return Ok(new GuessResultDto(solver.Guess()));
        }
    }
}
