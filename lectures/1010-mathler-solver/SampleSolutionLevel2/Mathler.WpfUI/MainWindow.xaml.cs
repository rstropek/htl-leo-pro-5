using Mathler.Solver;
using System.ComponentModel;
using System.Windows;

namespace Mathler.WpfUI;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel ViewModel;

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
    }

    private void OnSetExpectedResult(object sender, RoutedEventArgs e) => ViewModel.SetExpectedResult();
    private void Store(object sender, RoutedEventArgs e) => ViewModel.Store();
    private void Reset(object sender, RoutedEventArgs e) => ViewModel.Reset();
}

public interface IAlerter
{
    void DisplayAlertMessage(string message);
}

public class Alerter : IAlerter
{
    public void DisplayAlertMessage(string message)
    {
        MessageBox.Show(message);
    }
}

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IPuzzleSolverFactory factory;
    private readonly IAlerter alerter;

    public MainWindowViewModel(IPuzzleSolverFactory factory, IAlerter alerter)
    {
        this.factory = factory;
        this.alerter = alerter;
    }

    public IPuzzleSolver? Solver { get; private set; }

    public int? ExpectedResult { get; set; }

    public bool ExpectedResultSet => ExpectedResult != null;

    public string Formula { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public string Guess { get; set; } = string.Empty;

    public void SetExpectedResult()
    {
        if (ExpectedResult.HasValue)
        {
            Solver = factory.Create(ExpectedResult.Value);
            PropertyChanged?.Invoke(this, new(nameof(ExpectedResultSet)));
            SetGuess();
        }
    }

    public void Store()
    {
        if (Solver != null)
        {
            try
            {
                Solver.StoreResult(Formula, Result);
                SetGuess();
            }
            catch (Exception ex)
            {
                alerter.DisplayAlertMessage(ex.Message);
                return;
            }

            Result = string.Empty;
        }
    }

    internal void SetGuess()
    {
        if (Solver != null)
        {
            Guess = Solver.Guess();
            Formula = Guess;
            PropertyChanged?.Invoke(this, new(nameof(Guess)));
            PropertyChanged?.Invoke(this, new(nameof(Formula)));
        }
    }

    public void Reset()
    {
        ExpectedResult = null;
        Formula = Result = Guess = string.Empty;
        Solver = null;
        PropertyChanged?.Invoke(this, new(nameof(ExpectedResult)));
        PropertyChanged?.Invoke(this, new(nameof(ExpectedResultSet)));
        PropertyChanged?.Invoke(this, new(nameof(Formula)));
        PropertyChanged?.Invoke(this, new(nameof(Guess)));
        PropertyChanged?.Invoke(this, new(nameof(Result)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
