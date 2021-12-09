namespace MaturaTrainer.Api;

public class MaturaTrainerContext: DbContext
{
    public MaturaTrainerContext(DbContextOptions<MaturaTrainerContext> options) : base(options) { }

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();

    public DbSet<Quiz> Quizzes => Set<Quiz>();
}

public class Question
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public List<AnswerOption> Options { get; set; } = new();
}

public class AnswerOption
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    [JsonIgnore]
    public Question? Question { get; set; }

    [JsonIgnore]
    public int QuestionId { get; set; }
}

public class Quiz
{
    public int Id { get; set; }

    public DateTime QuizDateTime { get; set; }

    public int NumberOfAnsweredQuestions { get; set; }

    public int NumberOfCorrectAnswers { get; set; }
}
