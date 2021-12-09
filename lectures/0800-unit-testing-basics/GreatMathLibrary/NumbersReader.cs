namespace GreatMathLibrary;

//public abstract class ReaderBase
//{
//    public abstract Task<IEnumerable<int>> ReadNumbers();
//}

public interface IReaderBase
{
    Task<IEnumerable<int>> ReadNumbers();
}

internal class NumbersReader : IReaderBase
{
    public async Task<IEnumerable<int>> ReadNumbers()
    {
        var fileContent = await File.ReadAllTextAsync("numbers.txt");
        return fileContent.Split(',').Select(valueText => int.Parse(valueText));
    }
}
