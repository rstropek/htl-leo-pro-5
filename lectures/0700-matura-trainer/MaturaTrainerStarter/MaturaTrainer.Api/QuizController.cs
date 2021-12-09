namespace MaturaTrainer.Api;

// The code in this file is COMPLETE. You do not need to change anything.

[Route("api/quizzes")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly MaturaTrainerContext context;

    public record AddQuizDto(
        int NumberOfAnsweredQuestions,
        int NumberOfCorrectAnswers);

    public QuizController(MaturaTrainerContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Returns all quizzes
    /// </summary>
    [HttpGet(Name = nameof(GetQuizzes))]
    public ActionResult<IEnumerable<Quiz>> GetQuizzes()
    {
        return Ok(context.Quizzes);
    }

    /// <summary>
    /// Returns a quiz by ID.
    /// </summary>
    /// <param name="id">ID of the quiz to return</param>
    [ProducesResponseType(typeof(Quiz), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = nameof(GetQuizById))]
    public async Task<ActionResult<Quiz>> GetQuizById(int id)
    {
        var q = await context.Quizzes
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
        if (q == null) { return NotFound(); }
        return Ok(q);
    }

    /// <summary>
    /// Adds a given quiz to the DB
    /// </summary>
    /// <param name="newQuiz">Quiz to add</param>
    [HttpPost]
    [ProducesResponseType(typeof(Quiz), StatusCodes.Status201Created)]
    public async Task<ActionResult<Quiz>> Add(AddQuizDto newQuiz)
    {
        if (newQuiz.NumberOfCorrectAnswers > newQuiz.NumberOfAnsweredQuestions)
        {
            return BadRequest("Number of correct answers must not be greater than number of answered questions");
        }

        var q = new Quiz
        {
            QuizDateTime = DateTime.UtcNow,
            NumberOfAnsweredQuestions = newQuiz.NumberOfAnsweredQuestions,
            NumberOfCorrectAnswers = newQuiz.NumberOfCorrectAnswers
        };
        context.Quizzes.Add(q);

        await context.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetQuizById), new { id = q.Id }, q);
    }

    /// <summary>
    /// Delete a quiz with the given ID
    /// </summary>
    /// <param name="id">Quiz to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteQuiz(int id)
    {
        var q = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id);
        if (q == null) { return NotFound(); }
        context.Quizzes.Remove(q);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
