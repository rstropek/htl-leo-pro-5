using System.Collections.Generic;
using System.Linq;

namespace MyFirstUnitTests;

public class TestBasicStatistics
{
    private class TestReader : IReaderBase
    {
        public Task<IEnumerable<int>> ReadNumbers()
            => Task.FromResult(new[] { 1, 2 }.AsEnumerable());
    }

    [Fact]
    public async Task SumValueFromReader()
    {
        var statistics = new BasicStatistics(new TestReader());
        var result = await statistics.SumValueFromReader();
        Assert.Equal(3, result);
    }
}
