using CityCongestionCharge.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        // Note: Students have to know how to trigger data loading when window is loaded.
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

    public ObservableCollection<Detection> Detections { get; set; } = new();

    public CarTypeDescription? SelectedCarType { get; set; }
    public bool OnlyInside { get; set; } = false;
    public bool OnlyMultiCarDetections { get; set; } = false;
    public string LicensePlateFilter { get; set; } = string.Empty;

    private async void OnLoaded(object sender, RoutedEventArgs e) => await Refresh();

    private async void OnRefresh(object sender, RoutedEventArgs e) => await Refresh();

    private async Task Refresh()
    {
        Detections.Clear();

        // Note: Students should put filter logic in Data library and reuse code between
        //       web API and WPF.
        foreach (var detection in await context
            .FilteredDetections(SelectedCarType?.CarType, LicensePlateFilter, OnlyInside, OnlyMultiCarDetections)
            .ToListAsync())
        {
            Detections.Add(detection);
        }
    }

    private void DbAdmin(object sender, RoutedEventArgs e)
    {
        var userDialog = new DbAdminWindow(context)
        {
            Owner = this
        };
        userDialog.ShowDialog();
    }
}
