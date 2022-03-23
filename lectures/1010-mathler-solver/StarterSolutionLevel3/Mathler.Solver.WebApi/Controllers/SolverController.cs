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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a guess for an existing game
        /// </summary>
        /// <response code="404">Game not found</response>
        [HttpGet("{id}", Name = nameof(Guess))]
        public async Task<ActionResult<GuessResultDto>> Guess(int id)
        {
            throw new NotImplementedException();
        }
    }
}
