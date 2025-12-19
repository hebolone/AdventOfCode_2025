namespace AdventOfCode_2025.Days;

internal partial class Day09 : Day {

    public override object Basic() {
        var areas = DoWork();
        return areas.Max(r => r.Item3);
    }

    public override object Advanced() {
        return -1;
    }

    #region Protected

    protected override void Parse(List<string> input) {
        input.ForEach(line => {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            Coordinate coord = new(int.Parse(parts[0]), int.Parse(parts[1]));
            _RedTiles.Add(coord);
        });
    }

    #endregion

    #region Private

    private List<Coordinate> _RedTiles = [];

    private List<(Coordinate, Coordinate, ulong)> DoWork() {
        List<(Coordinate, Coordinate, ulong)> retValue = [];
        
        //  Get all possible rectangles
        var rectangles = CreateRectanglesTable();

        foreach(var (r1, r2) in rectangles) {
            ulong area = ((ulong) (Math.Abs(r2.X - r1.X) + 1)) * ((ulong) (Math.Abs(r2.Y - r1.Y) + 1));
            retValue.Add((r1, r2, area));
        }

        return retValue;
    }

    private List<(Coordinate r1, Coordinate r2)> CreateRectanglesTable() {
        List<(Coordinate r1, Coordinate r2)> rectangles = [];

        var redTiles = _RedTiles.Select((v, i) => new { Index = i, Value = v });
        foreach(var redTile1 in redTiles) {
            foreach(var redTile2 in redTiles.Where(b => b.Index > redTile1.Index)) {
                rectangles.Add((redTile1.Value, redTile2.Value));
            }
        }

        return rectangles;
    }

    #endregion

}