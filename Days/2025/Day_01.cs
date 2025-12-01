namespace AdventOfCode_2025.Days;

internal class Day01 : Day {

    public override object Basic() {
        int current = _Start;
        _Input.ForEach(line => {
            current = Rotation(current, line[0].ToString(), int.Parse(line[1..]));
            if(current == 0) {
                _Reset++;
            }
        });
        return _Reset;
    }

    public override object Advanced() {
        return -1;
    }

    #region Protected

    // protected override void Parse(List<string> input) {
    //     Regex regex = new(@"(?<left>\d+)\s+(?<right>\d+)", RegexOptions.Compiled);
    //     input.ForEach(l => {
    //         MatchCollection matches = regex.Matches(l);
    //         foreach (Match match in matches.Cast<Match>()) {
    //             GroupCollection groups = match.Groups;
    //             var left_value = int.Parse(groups["left"].Value);
    //             var right_value = int.Parse(groups["right"].Value);
    //             _Left.Add(left_value);
    //             _Right.Add(right_value);
    //         }
    //     });
    // }

    #endregion

    #region Private

    private int _Start = 50;
    private int _Max = 100;
    private int _Reset = 0;
    private int Rotation(int current, string direction, int steps) {
        int multiplier = direction.ToLower() switch {
            "l" => -1,
            "r" => 1,
            _ => throw new ArgumentException("Invalid direction")
        };
        var result = (current + (multiplier * steps)) % _Max;
        if(result < 0) {
            result += _Max;
        }
        return result;
    }

    #endregion

}