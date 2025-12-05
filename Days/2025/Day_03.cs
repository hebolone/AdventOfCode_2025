namespace AdventOfCode_2025.Days;

internal class Day03 : Day {

    public override object Basic() {
        List<int> highestVoltages = [];
        _Input.ForEach(line => {
            int highestVoltage = FindHighestVoltage(line);
            highestVoltages.Add(highestVoltage);
        });

        return highestVoltages.Sum();
    }

    public override object Advanced() {
        List<long> highestVoltages = [];
        _Input.ForEach(line => {
            long highestVoltage = FindHighestVoltageAdvanced(line);
            highestVoltages.Add(highestVoltage);
        });
        return highestVoltages.Sum();
    }

    #region Private

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

    private static long FindHighestVoltageAdvanced(string input) {
        const int MAXLENGTH = 12;
        //  Search highest value from 1 to 4
        int startIndex = 0;
        int valuesFound = 0;
        string result = string.Empty;

        while(valuesFound < MAXLENGTH) {
            var digitsLeft = input.Length - startIndex;
            var digitsToFind = MAXLENGTH - valuesFound;
            var stringToSearchIntoLength = digitsLeft - digitsToFind + 1;
            var stringToSearchInto = input.Substring(startIndex, stringToSearchIntoLength);
            
            var highestValueIndex = FindHighestSingleVoltagePositionAdvanced(stringToSearchInto);

            result += input[startIndex + highestValueIndex];

            startIndex += highestValueIndex + 1;
            valuesFound++;
        }

        Console.WriteLine(result);

        return long.Parse(result);
    }

    private static int FindHighestSingleVoltagePositionAdvanced(string input) {
        int highest = 0;
        int retValue = 0;

        for(int i = 0; i < input.Length; i++) {
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