namespace Sokoban.Logic;

public class SokobanGame
{
    private void FindPlayer()
    {
        var rowIx = 0;
        foreach (var row in Rows) {
            var cellIx = 0;
            foreach (var cell in row.Cells) {
                if (cell.Symbol == CellSymbol.Player)
                {
                    row.Cells[cellIx].Symbol = CellSymbol.Floor;
                    Player.Position = new(rowIx, cellIx);
                    return;
                }

                cellIx++;
            }

            rowIx++;
        }

        throw new InvalidOperationException("There is no player in this level");
    }

    public SokobanGame() { }

    public void StartLevel(string[] levelStructure)
    {
        Rows.Clear();
        foreach(var r in levelStructure)
        {
            Rows.Add(new(r));
        }

        FindPlayer();
    }

    public ObservableCollection<BoardRow> Rows { get; } = new();

    public Player Player { get; } = new();

    public void MovePlayer(PlayerDirection direction)
    {
        Player.Direction = direction;

        var (dx, dy) = (0, 0);
        switch (direction)
        {
            case PlayerDirection.Left: dx = -1; break;
            case PlayerDirection.Right: dx = 1; break;
            case PlayerDirection.Up: dy = -1; break;
            case PlayerDirection.Down: dy = 1; break;
            default: throw new NotImplementedException("Unknown direction");
        };

        var targetCell = Rows[Player.Position.RowIx + dy].Cells[Player.Position.CellIx + dx];
        switch (targetCell.Symbol)
        {
            case CellSymbol.Wall:
                return;
            case CellSymbol.Box or CellSymbol.BoxOnTarget:
                var nextBlockAfterTarget = Rows[Player.Position.RowIx + dy * 2].Cells[Player.Position.CellIx + dx * 2];
                if (nextBlockAfterTarget.Symbol is not CellSymbol.Floor and not CellSymbol.Target)
                {
                    return;
                }

                nextBlockAfterTarget.Symbol = nextBlockAfterTarget.Symbol switch
                {
                    CellSymbol.Floor => CellSymbol.Box,
                    CellSymbol.Target => CellSymbol.BoxOnTarget,
                    _ => throw new NotImplementedException("Unexpected target symbol"),
                };

                targetCell.Symbol = targetCell.Symbol switch
                {
                    CellSymbol.BoxOnTarget => CellSymbol.Target,
                    CellSymbol.Box => CellSymbol.Floor,
                    _ => throw new NotImplementedException("Unexpected target symbol")
                };

                break;
            default:
                break;
        }

        Player.Position = new(Player.Position.RowIx + dy, Player.Position.CellIx + dx);
    }
}

public class BoardRow
{
    public BoardRow(string rowStructure)
    {
        foreach (var c in rowStructure)
        {
            Cells.Add(new((CellSymbol)c));
        }
    }

    public ObservableCollection<BoardCell> Cells { get; } = new();
}

public enum CellSymbol
{
    Wall = 'X',
    Floor = ' ',
    Target= '.',
    Box = 'b',
    BoxOnTarget = 'B',
    Empty = '_',
    Player = '@',
}

public class BoardCell : INotifyPropertyChanged
{
    public BoardCell(CellSymbol symbol)
    {
        Symbol = symbol;
    }

    public string ImagePath => Symbol switch
    {
        CellSymbol.Wall => ImagePathBuilder.Build("Blocks/block_06.png"),
        CellSymbol.Floor or CellSymbol.Player => ImagePathBuilder.Build("Ground/ground_01.png"),
        CellSymbol.Target => ImagePathBuilder.Build("Ground/ground_04.png"),
        CellSymbol.Box => ImagePathBuilder.Build("Crates/crate_43.png"),
        CellSymbol.BoxOnTarget => ImagePathBuilder.Build("Crates/crate_08.png"),
        CellSymbol.Empty => ImagePathBuilder.Build("Ground/empty.png"),
        _ => throw new NotImplementedException("Unknown symbol")
    };

    private CellSymbol SymbolValue = CellSymbol.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public CellSymbol Symbol
    {
        get => SymbolValue;
        set
        {
            if (value != SymbolValue)
            {
                SymbolValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Symbol)));
                PropertyChanged?.Invoke(this, new(nameof(ImagePath)));
            }
        }
    }
}
