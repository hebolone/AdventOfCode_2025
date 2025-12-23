namespace AdventOfCode_2025.Days;

internal partial class Day09 : Day {

    public override object Basic() => DoWork();

    public override object Advanced() => DoWorkAdvanced();

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

    private readonly List<Coordinate> _RedTiles = [];
    private enum TOrientation { NW, NE, SE, SW };

    private ulong DoWork() {
        List<ulong> areas = [];
        
        //  Get all possible rectangles
        var rectangles = CreateRectanglesTable();

        foreach(var (r1, r2) in rectangles) {
            ulong area = ((ulong) (Math.Abs(r2.X - r1.X) + 1)) * ((ulong) (Math.Abs(r2.Y - r1.Y) + 1));
            areas.Add(area);
        }

        return areas.Max();
    }

    private ulong DoWorkAdvanced() {
        List<ulong> areas = [];

        //  Create fence around red tiles
        var fencedRedTiles = CreateFencedRedTiles();
        
        //  Get all possible rectangles
        var rectangles = CreateRectanglesTable();

        foreach(var (r1, r2) in rectangles) {
            ulong area = ((ulong) (Math.Abs(r2.X - r1.X) + 1)) * ((ulong) (Math.Abs(r2.Y - r1.Y) + 1));
            areas.Add(area);
        }

        return areas.Max();
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

    private List<(Coordinate, TOrientation)> CreateFencedRedTiles() {
        List<(Coordinate, TOrientation)> retValue = [];
        var start = _RedTiles.MinBy(t => t.X + t.Y)!;
        var completed = false;
        Coordinate current = start;

        do {
            retValue.Add((current, TOrientation.NW));
            current = SearchNext(current);
            completed = current == start;
            break;
        } while(!completed);

        return retValue;
    }

    private Coordinate SearchNext(Coordinate start) {
        //var east = _RedTiles.Where(t => t.Y == start.Y && t.X > start.X).MinBy(t => t.X);
        // var south = _RedTiles.Where(t => t.X == start.X && t.Y > start.Y).MinBy(t => t.Y);
        // var west = _RedTiles.Where(t => t.Y == start.Y && t.X < start.X).MaxBy(t => t.X);
        // var north = _RedTiles.Where(t => t.X == start.X && t.Y < start.Y).MaxBy(t => t.Y);

        List<Coordinate?> directions = [
            _RedTiles.Where(t => t.Y == start.Y && t.X > start.X).MinBy(t => t.X),  //  East
            _RedTiles.Where(t => t.X == start.X && t.Y > start.Y).MinBy(t => t.Y),  //  South
            _RedTiles.Where(t => t.Y == start.Y && t.X < start.X).MaxBy(t => t.X),  //  West
            _RedTiles.Where(t => t.X == start.X && t.Y < start.Y).MaxBy(t => t.Y)   //  North
        ];

        return directions.First(d => d is not null)!;
    } 
    
    #endregion

}