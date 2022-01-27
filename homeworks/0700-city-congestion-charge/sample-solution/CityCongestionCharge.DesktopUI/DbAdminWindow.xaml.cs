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

    public List<string?> Confirmations { get; } = new() { null, "Yes, I confirm" };

    public bool Confirmed => SelectedConfirmation != null;

    private string? selectedConfirmation = null;
    public string? SelectedConfirmation
    {
        get => selectedConfirmation;
        set
        {
            if (value != selectedConfirmation)
            {
                selectedConfirmation = value;
                PropertyChanged?.Invoke(this, new(nameof(SelectedConfirmation)));
                PropertyChanged?.Invoke(this, new(nameof(Confirmed)));
            }
        }
    }

    private async void OnClear(object sender, RoutedEventArgs e)
        => await CreateWriter().ClearAll();

    private async void OnFill(object sender, RoutedEventArgs e)
        => await CreateWriter().Fill();

    private DemoDataWriter CreateWriter()
    {
        var generator = new DemoDataGenerator();
        var writer = new DemoDataWriter(context, generator);
        return writer;
    }
}
