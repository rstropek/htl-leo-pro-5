using Mathler.Solver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Mathler.WpfUI;

public partial class App : Application
{
    private readonly IHost host;

    public App()
    {
        host = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IPuzzleSolverFactory, PuzzleSolverFactory>();
                services.AddSingleton<IFormulaEvaluator, FormulaEvaluator>();
                services.AddSingleton<IAlerter, Alerter>();
                services.AddTransient<MainWindowViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        await host.StartAsync();
        var mainWindow = host.Services.GetService<MainWindow>();

        mainWindow!.Show();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        using (host)
        {
            await host.StopAsync(TimeSpan.FromSeconds(5));
        }
    }
}
