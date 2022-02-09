namespace Sokoban;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ImagePathBuilder.Prefix = "Images/";
        DataContext = Game = new SokobanGame();
        Game.StartLevel(Levels.GetLevel(3));

        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Left or Key.Up or Key.Down or Key.Right)
        {
            Game.MovePlayer(KeyToDirection.Convert(e.Key));
        }
    }

    public SokobanGame Game { get; }
}
