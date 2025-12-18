namespace AdventOfCode_2025.Days;

internal partial class Day07 : Day {

    public override object Basic() => DoWork();

    public override object Advanced() => DoWorkAdvanced();

    #region Protected

    protected override void Parse(List<string> input) {
        int index = 0;
        _Board = new(input.First().Length, input.Count);

        input.ForEach(line => {
            foreach(char c in line) {
                _Board.SetValue(index, c);
                if(c == 'S') {
                    _Start = index;
                }
                index ++;
            }
        });
    }

    #endregion

    #region Private

    private Board<char>? _Board = null;
    private int _Start;
    private const char SPLITTER = '^';
    private const char BEAM = '|';
    private const char START = 'S';
    private const char EMPTY = '.';
    private class Beam(Coordinate position, ulong weight = 1) {
        public Coordinate Position => position;
        public ulong Weight { get; set; } = weight;
    }

    private int DoWork() {
        var splitGenerated = 0;

        //  Check current tile
        for(var y = 1; y < _Board!.Y; y ++) {
            for(int x = 0; x < _Board!.X; x ++) {
                var aboveTileCoord = new Coordinate(x, y - 1);
                var aboveTile = _Board[aboveTileCoord];
                switch(aboveTile) {
                    case START:
                    case BEAM:
                        if(_Board[x, y] == SPLITTER) {
                            var left = new Coordinate(x - 1, y);
                            CreateBeam(left);
                            var right = new Coordinate(x + 1, y);
                            CreateBeam(right);
                            splitGenerated++; 
                        } else {
                            CreateBeam(new Coordinate(x, y));
                        }
                        break;
                }
            }
        }

        return splitGenerated;
    }

    private void CreateBeam(Coordinate coordinate) {
        if(_Board![coordinate] == EMPTY) {
            _Board.SetValue(coordinate, BEAM);
        }
    }

    private ulong DoWorkAdvanced() {
        List<Beam> beams = [];

        //  Check current tile
        for(var y = 1; y < _Board!.Y; y ++) {
            for(int x = 0; x < _Board!.X; x ++) {
                var aboveTileCoord = new Coordinate(x, y - 1);
                var aboveTile = _Board[aboveTileCoord];
                var aboveBeam = GetBeam(beams, aboveTileCoord);
                if(aboveTile == START) {
                    beams.Add(new(new(x, y)));
                    _Board?.SetValue(new Coordinate(x, y), BEAM);
                } else if(aboveBeam != null) {
                    switch(aboveTile) {
                        case START:
                            break;
                        case BEAM:
                            if(_Board[x, y] == SPLITTER) {
                                var leftCoordinate = new Coordinate(x - 1, y);
                                CreateBeamAdvanced(beams, leftCoordinate, aboveBeam!.Weight);
                                var rightCoordinate = new Coordinate(x + 1, y);
                                CreateBeamAdvanced(beams, rightCoordinate, aboveBeam!.Weight);
                            } else {
                                CreateBeamAdvanced(beams, new (x, y), aboveBeam!.Weight);
                            }
                            break;
                    }
                }
            }
        }

        var bottomTiles = beams.Where(b => b.Position.Y == (_Board.Y - 1));
        ulong retValue = 0;
        foreach(var tile in bottomTiles) {
            retValue += tile.Weight;
        }
        return retValue;
    }

    private void CreateBeamAdvanced(List<Beam> beams, Coordinate coordinate, ulong incomingWeight) {
        var nearBeam = GetBeam(beams, coordinate);
        if(nearBeam == null) {
            beams.Add(new(coordinate, incomingWeight));
        } else {
            var nearBeamWeight = nearBeam?.Weight;
            nearBeam!.Weight += incomingWeight;
        }

        _Board?.SetValue(coordinate, BEAM);
    }

    private static Beam? GetBeam(List<Beam> beams, Coordinate coordinate) {
        return beams.FirstOrDefault(b => b.Position == coordinate);
    }

    #endregion

}