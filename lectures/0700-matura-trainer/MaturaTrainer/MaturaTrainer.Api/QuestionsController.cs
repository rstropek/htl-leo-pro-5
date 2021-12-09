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
        return Ok(context.Questions
            .Select(q => new QuestionSummaryDto { Id = q.Id, Text = q.Text, NumberOfOptions = q.Options.Count() })
            .Where(q => textFilter == null || textFilter == "" || q.Text.Contains(textFilter)));
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
        var q = await context.Questions
            .Include(q => q.Options)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
        if (q == null) { return NotFound(); }
        return Ok(q);
    }

    /// <summary>
    /// Returns a random question INCLUDING answer options.
    /// </summary>
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("random")]
    public async Task<ActionResult<Question>> GetRandom()
    {
        var numberOfQuestions = await context.Questions.CountAsync();
        if (numberOfQuestions == 0) { return NotFound(); }

        var rand = new Random();
        return await context.Questions
            .Skip(rand.Next(numberOfQuestions))
            .Include(q => q.Options)
            .AsNoTracking()
            .FirstAsync();
    }

    /// <summary>
    /// Adds a given question INCLUDING answer options to the DB
    /// </summary>
    /// <param name="newQuestion">Question to add</param>
    [HttpPost]
    [ProducesResponseType(typeof(Question), StatusCodes.Status201Created)]
    public async Task<ActionResult<Question>> Add(AddQuestionDto newQuestion)
    {
        var q = AddQuestion(newQuestion);
        await context.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetQuestionsById), new { id = q.Id }, q);
    }

    private Question AddQuestion(AddQuestionDto newQuestion)
    {
        var q = new Question
        {
            Text = newQuestion.Text,
            Options = newQuestion.Options.Select(o => new AnswerOption()
            {
                Text = o.Text,
                IsCorrect = o.IsCorrect,
            }).ToList()
        };
        context.Questions.Add(q);
        return q;
    }

    /// <summary>
    /// Adds multiple given questions INCLUDING answer options to the DB
    /// </summary>
    /// <param name="newQuestions">Questions to add</param>
    [HttpPost("bulk")]
    [ProducesResponseType(typeof(IEnumerable<Question>), StatusCodes.Status201Created)]
    public async Task<ActionResult<IEnumerable<Question>>> AddBulk(List<AddQuestionDto> newQuestions)
    {
        var questions = new List<Question>();
        foreach (var q in newQuestions)
        {
            questions.Add(AddQuestion(q));
        }
        
        await context.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetQuestions), questions);
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
        var q = await context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        if (q == null) { return NotFound(); }
        context.Questions.Remove(q);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
