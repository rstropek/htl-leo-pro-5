namespace AgeOfEmpiresTrainer.Api;

public class GameResultManager
{
    private readonly AoeTrainerContext context;

    public GameResultManager(AoeTrainerContext context)
    {
        this.context = context;
    }

    public Task AddGameResult(GameResult result)
    {
        // Todo: Add the code for storing the game result in the DB here.
        throw new NotImplementedException();
    }

    public Task<bool> TryDeleteGameResult(int id)
    {
        // Todo: Add the code for deleting the game result from the DB here.
        throw new NotImplementedException();
    }

    // Todo: Adjust the return type of the following function if necessary.
    public Task<IEnumerable<object>> GetGameResults(
        int? numberOfAIPlayers = null, Civilization? civilization = null, 
        GameStatus? gameStatus = null)
    {
        // Todo: Add the code for reading the game result from the DB here.
        throw new NotImplementedException();
    }
}
