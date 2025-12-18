namespace AdventOfCode_2025.Days;

internal partial class Day08 : Day {

    public override object Basic() {
        int maxConnections = IsTest ? 10 : 1000;
        return DoWork(maxConnections);
    }

    public override object Advanced() {
        return DoWorkAdvanced();
    }

    #region Protected

    protected override void Parse(List<string> input) {
        input.ForEach(line => {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            SpacialCoords spacialCoords = new(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
            JunctionBox junctionBox = new(spacialCoords);
            _JunctionBoxes.Add(junctionBox);
        });
    }

    #endregion

    #region Private

    private record SpacialCoords(int X, int Y, int Z) {
        public override string ToString()=> $"[{X},{Y},{Z}]";
    }
    private class JunctionBox(SpacialCoords coords) {
        public SpacialCoords Coords => coords;
        public int Circuit { get; set; } = 0;
    }
    private readonly List<JunctionBox> _JunctionBoxes = [];

    private int DoWork(int maxConnections) {
        var distanceTable = GetDistanceTable();
        var orderedDistances = distanceTable.OrderBy(r => r.distance).Take(maxConnections);
        int circuitID = 0;

        List<JunctionBox> circuits = [];

        foreach(var (boxStart, boxEnd, distance) in orderedDistances) {
            if(boxStart.Circuit == 0 && boxEnd.Circuit == 0) {
                //  Create new circuit
                var currentCircuitID = ++circuitID;
                boxStart.Circuit = currentCircuitID;
                boxEnd.Circuit = currentCircuitID;
            } else if(boxStart.Circuit == 0 && boxEnd.Circuit > 0) {
                boxStart.Circuit = boxEnd.Circuit;
            } else if(boxStart.Circuit > 0 && boxEnd.Circuit == 0) {
                boxEnd.Circuit = boxStart.Circuit;
            } else if(boxStart.Circuit != boxEnd.Circuit) {
                //  Merge 2 circuits :-)
                var boxesToMerge = _JunctionBoxes.Where(b => b.Circuit == boxEnd.Circuit);                
                // foreach(var boxToMerge in boxesToMerge) {
                //     boxToMerge.Circuit = boxStart.Circuit;
                // }

                boxesToMerge.ToList().ForEach(b => b.Circuit = boxStart.Circuit);
            }
        }

        //  Final calculation
        var retValue = 1;
        var circuitSizes = _JunctionBoxes.Where(i => i.Circuit > 0).GroupBy(b => b.Circuit);
        var topThreeCircuits = circuitSizes.OrderByDescending(c => c.Count()).Take(3);
        topThreeCircuits.ToList().ForEach(c => retValue *= c.Count());

        return retValue;
    }

    private int DoWorkAdvanced() {
        var distanceTable = GetDistanceTable();
        var orderedDistances = distanceTable.OrderBy(r => r.distance);
        int circuitID = 0;
        int retValue = 0;

        List<JunctionBox> circuits = [];

        foreach(var (boxStart, boxEnd, distance) in orderedDistances) {
            if(boxStart.Circuit == 0 && boxEnd.Circuit == 0) {
                //  Create new circuit
                var currentCircuitID = ++circuitID;
                boxStart.Circuit = currentCircuitID;
                boxEnd.Circuit = currentCircuitID;
            } else if(boxStart.Circuit == 0 && boxEnd.Circuit > 0) {
                boxStart.Circuit = boxEnd.Circuit;
            } else if(boxStart.Circuit > 0 && boxEnd.Circuit == 0) {
                boxEnd.Circuit = boxStart.Circuit;
            } else if(boxStart.Circuit != boxEnd.Circuit) {
                //  Merge 2 circuits :-)
                var boxesToMerge = _JunctionBoxes.Where(b => b.Circuit == boxEnd.Circuit);                
                boxesToMerge.ToList().ForEach(b => b.Circuit = boxStart.Circuit);
            }

            //  Check if this is the final connection :-)
            var circuitSizes = _JunctionBoxes.Where(i => i.Circuit > 0).GroupBy(b => b.Circuit);
            if(circuitSizes.Count() == 1 && circuitSizes.First().Count() == _JunctionBoxes.Count) {
                retValue = boxStart.Coords.X * boxEnd.Coords.X;
                break;
            }
        }

        return retValue;
    }

    private List<(JunctionBox boxStart, JunctionBox boxEnd, double distance)> GetDistanceTable() {
        List<(JunctionBox, JunctionBox, double)> retValue = [];

        var boxes = _JunctionBoxes.Select((v, i) => new { Index = i, Value = v });
        foreach(var boxStart in boxes) {
            foreach(var boxEnd in boxes.Where(b => b.Index > boxStart.Index)) {
                var distance = CalculateDistance(boxStart.Value.Coords, boxEnd.Value.Coords);
                retValue.Add((boxStart.Value, boxEnd.Value, distance));
            }
        }

        return retValue;
    }

    private static double CalculateDistance(SpacialCoords start, SpacialCoords end) {
        double dx = end.X - start.X;
        double dy = end.Y - start.Y;
        double dz = end.Z - start.Z;

        double sumOfSquares = (dx * dx) + (dy * dy) + (dz * dz);

        return sumOfSquares;
    }

    #endregion

}