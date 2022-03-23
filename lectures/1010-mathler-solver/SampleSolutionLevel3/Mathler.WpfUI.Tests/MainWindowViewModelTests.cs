using Mathler.Solver;
using Moq;
using Xunit;

namespace Mathler.WpfUI.Tests;

public class MainWindowViewModelTests
{
    [Fact]
    public void SetExpectedResult()
    {
        // Create mock object for solver
        var solverMock = new Mock<IPuzzleSolver>();
        solverMock.Setup(m => m.Guess()).Returns("12+34");

        // Create mock object for factory
        var factoryMock = new Mock<IPuzzleSolverFactory>();
        factoryMock.Setup(f => f.Create(42)).Returns(solverMock.Object);

        // Create ViewModel and store all property changes
        var mwvm = new MainWindowViewModel(factoryMock.Object, Mock.Of<IAlerter>()) { ExpectedResult = 42 };
        var notifications = new List<string>();
        mwvm.PropertyChanged += (_, ea) => notifications.Add(ea.PropertyName!);

        // Execute method to be tested
        mwvm.SetExpectedResult();

        // Make sure all expected property change events had been fired
        Assert.Equal(new[]
        {
            nameof(MainWindowViewModel.ExpectedResultSet),
            nameof(MainWindowViewModel.Guess),
            nameof(MainWindowViewModel.Formula),
        }.OrderBy(n => n), notifications.OrderBy(n => n));

        // Make sure that fields contain correct values
        Assert.Equal("12+34", mwvm.Formula);
        Assert.Equal("12+34", mwvm.Guess);
        Assert.NotNull(mwvm.ExpectedResult);
        Assert.True(mwvm.ExpectedResultSet);

        // Make sure that solver and factory methods had been called
        factoryMock.VerifyAll();
        solverMock.VerifyAll();
    }

    [Fact]
    public void StoreWithError()
    {
        // Create mock object for solver
        var solverMock = new Mock<IPuzzleSolver>();
        solverMock.Setup(m => m.StoreResult("12a34", "?????")).Throws(new Exception("Exception"));

        // Create mock object for factory
        var factoryMock = new Mock<IPuzzleSolverFactory>();
        factoryMock.Setup(f => f.Create(42)).Returns(solverMock.Object);

        // Create mock for alerter
        var alerterMock = new Mock<IAlerter>();
        alerterMock.Setup(m => m.DisplayAlertMessage("Exception"));

        // Create ViewModel
        var mwvm = new MainWindowViewModel(factoryMock.Object, alerterMock.Object) { ExpectedResult = 42 };
        mwvm.SetExpectedResult();
        mwvm.Formula = "12a34";
        mwvm.Result = "?????";

        // Execute method to test
        mwvm.Store();

        // Make sure that alert had been displayed
        alerterMock.VerifyAll();
    }

    [Fact]
    public void StoreWithSuccess()
    {
        // Create mock object for solver
        var solverMock = new Mock<IPuzzleSolver>();
        solverMock.Setup(m => m.StoreResult("12+34", "?????"));
        solverMock.Setup(m => m.Guess()).Returns("34+56");

        // Create mock object for factory
        var factoryMock = new Mock<IPuzzleSolverFactory>();
        factoryMock.Setup(f => f.Create(42)).Returns(solverMock.Object);

        // Create mock for alerter
        var alerterMock = new Mock<IAlerter>();

        // Create ViewModel and store all property changes
        var mwvm = new MainWindowViewModel(factoryMock.Object, alerterMock.Object) { ExpectedResult = 42 };
        mwvm.SetExpectedResult();
        mwvm.Formula = "12+34";
        mwvm.Result = "?????";
        var notifications = new List<string>();
        mwvm.PropertyChanged += (_, ea) => notifications.Add(ea.PropertyName!);

        // Execute method to test
        mwvm.Store();

        // Make sure all expected property change events had been fired
        Assert.Equal(new[]
        {
            nameof(MainWindowViewModel.Guess),
            nameof(MainWindowViewModel.Formula),
        }.OrderBy(n => n), notifications.OrderBy(n => n));

        // Make sure that fields contain correct values
        Assert.Equal("34+56", mwvm.Formula);
        Assert.Equal("34+56", mwvm.Guess);

        // Make sure that solver and factory methods had been called
        factoryMock.VerifyAll();
        solverMock.VerifyAll();

        // Make sure that no alert had been displayed
        alerterMock.Verify(m => m.DisplayAlertMessage(It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void Reset()
    {
        // Create ViewModel
        var mwvm = new MainWindowViewModel(Mock.Of<IPuzzleSolverFactory>(), Mock.Of<IAlerter>()) { ExpectedResult = 42 };
        var notifications = new List<string>();
        mwvm.PropertyChanged += (_, ea) => notifications.Add(ea.PropertyName!);

        // Execute method to test
        mwvm.Reset();

        // Make sure all expected property change events had been fired
        Assert.Equal(new[]
        {
            nameof(MainWindowViewModel.Guess),
            nameof(MainWindowViewModel.Formula),
            nameof(MainWindowViewModel.ExpectedResult),
            nameof(MainWindowViewModel.ExpectedResultSet),
            nameof(MainWindowViewModel.Result),
        }.OrderBy(n => n), notifications.OrderBy(n => n));

        // Make sure that fields contain correct values
        Assert.True(string.IsNullOrEmpty(mwvm.Formula));
        Assert.True(string.IsNullOrEmpty(mwvm.Guess));
        Assert.True(string.IsNullOrEmpty(mwvm.Result));
        Assert.Null(mwvm.ExpectedResult);
        Assert.False(mwvm.ExpectedResultSet);
    }
}
