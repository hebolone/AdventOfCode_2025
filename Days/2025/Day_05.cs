using System.Reflection.Metadata;

namespace AdventOfCode_2025.Days;

internal class Day05 : Day {

    public override object Basic() {
        var ingredientsInRange = 0;
        _Ingredients.ForEach(ingredient => {
            if(IsInRange(ingredient)) {
                ingredientsInRange++;
            }
        });
        return ingredientsInRange;
    }

    public override object Advanced() {
        ulong retValue = 0;
        //  Sort ranges
        _IDRanges.Sort((r1, r2) => r1.Start.CompareTo(r2.Start));
        //  Procede to merge
        var mergedRanges = new List<IDRange>();
        IDRange currentRange = _IDRanges[0];

        for(int i = 1; i < _IDRanges.Count; i++) {
            var nextRange = _IDRanges[i];
            var intersectedRanges = IntersectRanges(currentRange, nextRange);
            if(intersectedRanges.Count == 1) {
                //  Merged
                currentRange = intersectedRanges[0];
            } else {
                //  Not merged
                mergedRanges.Add(currentRange);
                currentRange = nextRange;
            }
        }

        mergedRanges.Add(currentRange);
        mergedRanges.ForEach(range => {
            retValue += (range.End - range.Start + 1);
        });

        return retValue;
    }

    #region Protected

    protected override void Parse(List<string> input) {
        foreach (var line in input) {
            if(line.Contains('-')) {
                var parts = line.Split('-');
                var start = ulong.Parse(parts[0]);
                var end = ulong.Parse(parts[1]);
                _IDRanges.Add(new IDRange(start, end));
            } else if(!string.IsNullOrEmpty(line)) {
                _Ingredients.Add(ulong.Parse(line));
            }
        }
    }

    #endregion

    #region Private

    private record IDRange(ulong Start, ulong End);
    private readonly List<IDRange> _IDRanges = [];
    private readonly List<ulong> _Ingredients = [];

    private bool IsInRange(ulong ingredient) {
        foreach(var range in _IDRanges) {
            if(ingredient >= range.Start && ingredient <= range.End) {
                return true;
            }
        }
        return false;
    }

    private bool AreRangesOverlapped(IDRange range1, IDRange range2) {
        if(range1.Start <= range2.End && range1.End >= range2.Start) {
            return true;
        }
        return false;
    }

    private List<IDRange> IntersectRanges(IDRange range1, IDRange range2) {
        var intersectedRanges = new List<IDRange>();

        if(!AreRangesOverlapped(range1, range2)) {
            intersectedRanges.Add(range1);
            intersectedRanges.Add(range2);
            return intersectedRanges;
        }

        var start = Math.Min(range1.Start, range2.Start);
        var end = Math.Max(range1.End, range2.End);
        intersectedRanges.Add(new IDRange(start, end));
        return intersectedRanges;
    } 

    #endregion

}