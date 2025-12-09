using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

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

    private int DoWorkAdvanced() {
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

    #endregion

}