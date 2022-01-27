using CityCongestionCharge.Data;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly CccDataContext context;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        var factory = new CccDataContextFactory();
        context = factory.CreateDbContext();

        Loaded += OnLoaded;

        SelectedCarType = CarTypes[0];

    }

    public class CarTypeDescription
    {
        public CarType? CarType { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public List<CarTypeDescription> CarTypes { get; } = new()
    {
        new() { CarType = null, Description = "All" },
        new() { CarType = CarType.PassengerCar, Description = "Passenger Car" },
        new() { CarType = CarType.Van, Description = "Van" },
        new() { CarType = CarType.Lorry, Description = "Lorry" },
        new() { CarType = CarType.Motorcycle, Description = "Motorcycle" },
    };

    public CarTypeDescription? SelectedCarType { get; set; }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
