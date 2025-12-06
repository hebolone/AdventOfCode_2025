using System.Data;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace AdventOfCode_2025.Days;

internal partial class Day06 : Day {

    public override object Basic() {
        ulong result = 0;


        for(int column = 0; column < _Numbers.First().Value.Count; column ++) {
            List<int> numbers = [];
            for(int row = 0; row < _Numbers.Count; row ++) {
                numbers.Add(_Numbers[row][column]);
            }
            var partial = PerformOperation(numbers, _Operands[column]);
            result += partial;
        }

        return result;
    }

    public override object Advanced() => -1;

    #region Protected

    protected override void Parse(List<string> input) {
        int rowID = 0;
        List<char> availableOperands = ['+', '*'];
        foreach (var line in input) {
            //  Is this a row of numbers or a row of operands?
            char firstChar = line[0];
            if(availableOperands.Contains(firstChar)) {
                Regex regexOperands = RegexOperands();
                MatchCollection matches = regexOperands.Matches(line);
                foreach (Match match in matches) {
                    _Operands.Add(match.Value[0]);
                }
            } else {
                Regex regexNumbers = RegexNumbers();
                MatchCollection matches = regexNumbers.Matches(line);
                List<int> row = [];
                foreach (Match match in matches) {
                    row.Add(int.Parse(match.Value));
                }
                _Numbers.Add(rowID, row);
            }
            rowID++;
        }
    }

    #endregion

    #region Private

    private readonly Dictionary<int, List<int>> _Numbers = [];
    private readonly List<char> _Operands = [];

    [GeneratedRegex(@"\b\d+\b")]
    private static partial Regex RegexNumbers();

    [GeneratedRegex(@"[*+]+")]
    private static partial Regex RegexOperands();

    private static ulong PerformOperation(List<int> numbers, char operand) {
        ulong retValue = (ulong) (operand == '+' ? 0 : 1);

        for(int i = 0; i < numbers.Count; i ++) {
            switch(operand) {
                case '+':
                    retValue += (ulong) numbers[i];
                    break;
                case '*':
                    retValue *= (ulong) numbers[i];
                    break;
            }
        }

        return retValue;
    }

    #endregion

}