using CityCongestionCharge.Data;
using System.ComponentModel;
using System.Windows;

namespace CityCongestionCharge.DesktopUI;

public partial class DbAdminWindow : Window
{
    private readonly CccDataContext context;

    public DbAdminWindow(CccDataContext context)
    {
        InitializeComponent();
        DataContext = this;

        this.context = context;
    }

    public List<string> Confirmations { get; } = new() { string.Empty, "Yes, I confirm" };

    private DemoDataWriter CreateWriter()
    {
        var generator = new DemoDataGenerator();
        var writer = new DemoDataWriter(context, generator);
        return writer;
    }
}
