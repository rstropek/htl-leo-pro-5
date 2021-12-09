namespace MaturaTrainer.Api;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly MaturaTrainerContext context;

    public class QuestionSummaryDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = String.Empty;
        public int NumberOfOptions { get; set; }
    }

    public record AddQuestionDto(
        [Required][MinLength(5)] string Text,
        [MinLength(1)] AddAnswerOptionDto[] Options);

    public record AddAnswerOptionDto(
        [Required][MinLength(2)] string Text,
        bool IsCorrect);

    public QuestionsController(MaturaTrainerContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Returns all questions
    /// </summary>
    /// <param name="textFilter">(Optional) Question text filter</param>
    /// <remarks>
    /// If <paramref name="textFilter"/> is not null, only questions will be returned
    /// whose text contains the value specified in <paramref name="textFilter"/>.
    /// </remarks>
    [HttpGet(Name = nameof(GetQuestions))]
    public ActionResult<IEnumerable<QuestionSummaryDto>> GetQuestions(
        [FromQuery(Name = "q")] string? textFilter)
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }

    /// <summary>
    /// Returns a question INCLUDING answer options.
    /// </summary>
    /// <param name="id">ID of the question to return</param>
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = nameof(GetQuestionsById))]
    public async Task<ActionResult<Question>> GetQuestionsById(int id)
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }

    /// <summary>
    /// Returns a RANDOM question INCLUDING answer options.
    /// </summary>
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("random")]
    public async Task<ActionResult<Question>> GetRandom()
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }

    /// <summary>
    /// Adds a given question INCLUDING answer options to the DB
    /// </summary>
    /// <param name="newQuestion">Question to add</param>
    [HttpPost]
    [ProducesResponseType(typeof(Question), StatusCodes.Status201Created)]
    public async Task<ActionResult<Question>> Add(AddQuestionDto newQuestion)
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }

    /// <summary>
    /// Adds multiple given questions INCLUDING answer options to the DB
    /// </summary>
    /// <param name="newQuestions">Questions to add</param>
    [HttpPost("bulk")]
    [ProducesResponseType(typeof(IEnumerable<Question>), StatusCodes.Status201Created)]
    public async Task<ActionResult<IEnumerable<Question>>> AddBulk(List<AddQuestionDto> newQuestions)
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }

    /// <summary>
    /// Delete a question with the given ID
    /// </summary>
    /// <param name="id">Question to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteQuestion(int id)
    {
        throw new NotImplementedException("Todo: Implement this functionality");
    }
}
