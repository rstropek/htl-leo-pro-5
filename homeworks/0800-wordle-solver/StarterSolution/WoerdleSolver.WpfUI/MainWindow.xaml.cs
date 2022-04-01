using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WoerdleSolver.Logic;

namespace WoerdleSolver.WpfUI;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public MainWindow()
    {
        InitializeComponent();
    }

    // Todo: Check if the data type of the following property is suitable. Change it if necessary!
    #warning Check if the data type of the following property is suitable. Change it if necessary!
    public List<string> PossibleWords { get; } = new();

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
}
