using System.Reflection.Metadata;

namespace AdventOfCode_2025.Days;

internal class Day04 : Day {

    public override object Basic() => GetRemovableRollsOfPaper().Count;

    public override object Advanced() {
        
        return WorkAdvanced();
    }

    #region Protected

    protected override void Parse(List<string> input) {
        int width = input.First().Length;
        int height = input.Count;
        _Board = new(width, height);
        int index = 0;
        foreach(var row in input) {
            foreach(var c in row) {
                _Board.SetValue(index, c);
                index++;
            }
        }
    }

    #endregion

    #region Private

    private Board<char>? _Board = null;
    private char _RollOfPaper = '@';

    private List<Coordinate> GetRemovableRollsOfPaper() {
        List<Coordinate> retValue = [];
        const int MAXROLLSOFPAPER = 4;
        
        for(int i = 0; i < _Board?.Count(); i ++) {
            var adjacents = _Board?.GetSurrounding(i);
            int rollsOfPaper = 0;
            if(_Board?[i] == _RollOfPaper) {
                foreach(var adjacent in adjacents) {
                    if(_Board?[adjacent] == _RollOfPaper) {
                        rollsOfPaper ++;
                    }
                }
                if(rollsOfPaper < MAXROLLSOFPAPER) {
                    retValue.Add(_Board?.ConvertIndexToCoordinates(i));
                }
            }
        }

        return retValue;
    }

    private int WorkAdvanced() {
        var removed = 0;
        List<Coordinate> removables = [];
        do {
            removables = GetRemovableRollsOfPaper();
            RemoveProcess(removables);
            removed += removables.Count;
        } while(removables.Count > 0);
        return removed;
    }

    private void RemoveProcess(List<Coordinate> removables) {
        removables.ForEach(r => {
            _Board?.SetValue(r, 'x');
        });
    }

    #endregion

}