namespace GreatMathLibrary;

public class BasicStatistics
{
    private readonly IReaderBase reader;

    public BasicStatistics(IReaderBase reader)
    {
        this.reader = reader;
    }

    public async Task<int> SumValueFromReader()
    {
        var numbers = await reader.ReadNumbers();
        return numbers.Sum();
    }
}
