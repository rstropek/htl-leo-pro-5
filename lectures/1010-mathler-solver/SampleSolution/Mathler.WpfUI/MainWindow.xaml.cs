using Mathler.Solver;
using System;
using System.ComponentModel;
using System.Windows;

namespace Mathler.WpfUI;

public partial class MainWindow : Window
{
    private readonly MainWindowViewModel ViewModel;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = ViewModel = new MainWindowViewModel();
    }

    private void OnSetExpectedResult(object sender, RoutedEventArgs e) => ViewModel.SetExpectedResult();
    private void Store(object sender, RoutedEventArgs e) => ViewModel.Store();
    private void Reset(object sender, RoutedEventArgs e) => ViewModel.Reset();
}

internal class MainWindowViewModel : INotifyPropertyChanged
{
    public PuzzleSolver? Solver { get; private set; }

    private int? ExpectedResultValue;
    public int? ExpectedResult
    {
        get => ExpectedResultValue;
        set
        {
            if (ExpectedResultValue != value)
            {
                ExpectedResultValue = value;
                PropertyChanged?.Invoke(this, new(nameof(ExpectedResult)));
            }
        }
    }

    public bool ExpectedResultSet
    { 
        get => ExpectedResultValue != null;
    }

    private string FormulaValue = string.Empty;
    public string Formula
    {
        get => FormulaValue;
        set
        {
            if (FormulaValue != value)
            {
                FormulaValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Formula)));
            }
        }
    }

    private string ResultValue = string.Empty;
    public string Result
    {
        get => ResultValue;
        set
        {
            if (ResultValue != value)
            {
                ResultValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Result)));
            }
        }
    }

    private string GuessValue = string.Empty;
    public string Guess
    {
        get => GuessValue;
        set
        {
            if (GuessValue != value)
            {
                GuessValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Guess)));
            }
        }
    }

    public void SetExpectedResult()
    {
        if (ExpectedResultValue.HasValue)
        {
            Solver = new(ExpectedResultValue.Value);
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
                MessageBox.Show(ex.Message);
                return;
            }

            Result = string.Empty;
        }
    }

    public void SetGuess()
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
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
