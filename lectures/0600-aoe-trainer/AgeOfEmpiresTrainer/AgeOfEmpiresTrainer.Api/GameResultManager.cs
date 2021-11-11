namespace AgeOfEmpiresTrainer.Api;

public class GameResultManager
{
    private readonly AoeTrainerContext context;

    public GameResultManager(AoeTrainerContext context)
    {
        this.context = context;
    }

    public async Task AddGameResult(GameResult result)
    {
        // Todo: Add the code for storing the game result in the DB here.
        //throw new NotImplementedException();
        #region Solution
        if (result.AIPlayers != null && result.NumberOfAIPlayers != result.AIPlayers.Count)
        {
            throw new ArgumentException("Wrong number of AI players", nameof(result));
        }

        context.GameResults.Add(result);
        await context.SaveChangesAsync();
        #endregion
    }

    public async Task<bool> TryDeleteGameResult(int id)
    {
        // Todo: Add the code for deleting the game result from the DB here.
        //throw new NotImplementedException();
        #region Solution
        var game = await context.GameResults.FirstOrDefaultAsync(gr => gr.Id == id);
        if (game == null)
        {
            return false;
        }

        context.GameResults.Remove(game);
        await context.SaveChangesAsync();
        return true;
        #endregion
    }

    #region Solution
    public record GameResultSummary(int Id, int NumberOfAIPlayers, 
        string Civilizations, string GameStatus);
    #endregion

    public async Task<IEnumerable<GameResultSummary>> GetGameResults(
        int? numberOfAIPlayers = null, Civilization? civilization = null, 
        GameStatus? gameStatus = null)
    {
        // Todo: Add the code for reading the game result from the DB here.
        //throw new NotImplementedException();
        #region Solution
        IQueryable<GameResult> result = context.GameResults;

        if (numberOfAIPlayers.HasValue)
        {
            result = result.Where(gr => gr.NumberOfAIPlayers == numberOfAIPlayers);
        }

        if (civilization.HasValue)
        {
            result = result.Where(gr => gr.AIPlayers.Any(ai => ai.Civilization == civilization));
        }

        if (gameStatus.HasValue)
        {
            result = result.Where(gr => gr.GameStatus == gameStatus);
        }

        return await result
            .OrderByDescending(gr => gr.Id)
            .Select(gr => new GameResultSummary(
                gr.Id,
                gr.NumberOfAIPlayers,
                string.Join(", ", gr.AIPlayers.Select(ai => ai.Civilization.ToString())),
                gr.GameStatus.ToString()))
            .ToListAsync();
        #endregion
    }
}
