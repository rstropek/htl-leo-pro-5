using CityCongestionCharge.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

public partial class DbAdminWindow : Window, INotifyPropertyChanged
{
    private readonly CccDataContext context;

    public event PropertyChangedEventHandler? PropertyChanged;

    public DbAdminWindow(CccDataContext context)
    {
        InitializeComponent();
        DataContext = this;

        this.context = context;
    }

    public List<string> Confirmations { get; } = new() { string.Empty, "Yes, I confirm" };

    public bool DbActionsEnabled => !string.IsNullOrEmpty(SelectedConfirmation) && !IsRunning;

    private bool isRunning = false;
    public bool IsRunning
    {
        get => isRunning;
        set
        {
            if (value != isRunning)
            {
                isRunning = value;
                PropertyChanged?.Invoke(this, new(nameof(DbActionsEnabled)));
            }
        }
    }

    private string selectedConfirmation = string.Empty;
    public string SelectedConfirmation
    {
        get => selectedConfirmation;
        set
        {
            if (value != selectedConfirmation)
            {
                selectedConfirmation = value;
                PropertyChanged?.Invoke(this, new(nameof(SelectedConfirmation)));
                PropertyChanged?.Invoke(this, new(nameof(DbActionsEnabled)));
            }
        }
    }

    private async void OnClear(object sender, RoutedEventArgs e)
    {
        IsRunning = true;
        try
        {
            await CreateWriter().ClearAll();
            MessageBox.Show("DB has been cleared", "CCC", MessageBoxButton.OK);
        }
        finally
        {
            IsRunning = false;
        }
    }

    private async void OnFill(object sender, RoutedEventArgs e)
    {
        IsRunning = true;
        try
        {
            await CreateWriter().Fill();
            MessageBox.Show("DB has been filled", "CCC", MessageBoxButton.OK);
        }
        finally
        {
            IsRunning = false;
        }
    }

    private DemoDataWriter CreateWriter()
    {
        var generator = new DemoDataGenerator();
        var writer = new DemoDataWriter(context, generator);
        return writer;
    }
}
