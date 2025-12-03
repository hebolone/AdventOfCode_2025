namespace AdventOfCode_2025.Days;

internal class Day03 : Day {

    public override object Basic() {
        return _HighestVoltages.Values.Sum();
    }

    public override object Advanced() {
        return -1;
    }

    #region Protected

    protected override void Parse(List<string> input) {
        int i = 0;
        input.ForEach(line => {
            int highestVoltage = FindHighestVoltage(line);
            _HighestVoltages.Add(i, highestVoltage);
            i++;
        });
    }

    #endregion

    #region Private

    private readonly Dictionary<int, int> _HighestVoltages = [];

    private static int FindHighestVoltage(string input) {
        int position = FindHighestSingleVoltagePosition(input);
        int secondPosition = FindHighestSingleVoltagePosition(input, position + 1);

        int retValue = int.Parse($"{input[position]}{input[secondPosition]}");

        return retValue;
    }

    private static int FindHighestSingleVoltagePosition(string input, int startIndex = 0) {
        int highest = 0;
        int retValue = 0;

        int avoidLastCharacter = startIndex == 0 ? 1 : 0;

        for(int i = startIndex; i < input.Length - avoidLastCharacter; i++) {
            int v = int.Parse(input[i].ToString());
            if(v > highest) {
                highest = v;
                retValue = i;
            }
        }
        return retValue;
    }

    #endregion

}