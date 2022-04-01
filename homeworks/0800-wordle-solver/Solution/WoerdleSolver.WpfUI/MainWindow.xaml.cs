using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WoerdleSolver.Logic;

namespace WoerdleSolver.WpfUI;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private Solver Solver = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainWindow()
    {
        InitializeComponent();

        FillGuesses();

        DataContext = this;
    }

    public ObservableCollection<string> PossibleWords { get; } = new();

    private string GuessedWordValue = string.Empty;
    public string GuessedWord
    {
        get => GuessedWordValue;
        set
        {
            if (value != GuessedWordValue)
            {
                GuessedWordValue = value;
                PropertyChanged?.Invoke(this, new(nameof(GuessedWord)));
            }
        }
    }

    private string ResultValue = string.Empty;
    public string Result
    {
        get => ResultValue;
        set
        {
            if (value != ResultValue)
            {
                ResultValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Result)));
            }
        }
    }

    private string? SelectedWordValue;
    public string? SelectedWord
    {
        get => SelectedWordValue;
        set
        {
            if (value != SelectedWordValue)
            {
                SelectedWordValue = value;
                PropertyChanged?.Invoke(this, new(nameof(SelectedWord)));
                if (value != null)
                {
                    GuessedWord = value;
                }
            }
        }
    }

    private void Store(object sender, RoutedEventArgs e)
    {
        try
        {
            Solver.StoreResult(GuessedWord, Result);
            FillGuesses();
            GuessedWord = Result = string.Empty;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void Reset(object sender, RoutedEventArgs e)
    {
        Solver = new();
        FillGuesses();
        GuessedWord = Result = string.Empty;
    }

    private void FillGuesses()
    {
        PossibleWords.Clear();
        foreach(var word in Solver.PossibleWords())
        {
            PossibleWords.Add(word);
        }
    }

}
