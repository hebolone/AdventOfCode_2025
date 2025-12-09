namespace AdventOfCode_2025.Days;

internal partial class Day06 : Day {

    public override object Basic() {
        ulong result = 0;

        for(int index = 0; index < _NumbersString.First().Value.Count; index ++) {
            List<int> numbers = [];
            for(int row = 0; row < _NumbersString.Count; row ++) {
                numbers.Add(int.Parse(_NumbersString[row][index]));
            }
            result += PerformOperation(numbers, _OperandsDatas[index].Operand);
        }

        return result;
    }

    public override object Advanced() {
        ulong result = 0;

        for(int index = 0; index < _OperandsDatas.Count; index ++) {
            List<string> numbers = [];
            for(int row = 0; row < _NumbersString.Count; row++) {
                numbers.Add(_NumbersString[row][index]);
            }

            var partial = PerformOperationAdvanced(numbers, _OperandsDatas[index].Operand);
            result += partial;
        }

        return result;
    }

    #region Protected

    protected override void Parse(List<string> input) {
        string operandsLine = input.Last();
        var lineLength = operandsLine.Length;

        //  Parse operands
        List<char> availableOperands = ['+', '*'];
        for(int i = 0; i < lineLength; i ++) {
            if(availableOperands.Contains(operandsLine[i])) {
                char operand = operandsLine[i];
                _OperandsDatas.Add(new(operand, i));
            }         
        }

        for(int i = 0; i < input.Count - 1; i ++) {
            _NumbersString.Add(i, []);
        }

        //  Parse values
        for(int op = 0; op < _OperandsDatas.Count; op ++) {
            int spacing;
            if (op == _OperandsDatas.Count - 1) {
                spacing = operandsLine.Length - _OperandsDatas[op].Position;
            } else {
                spacing = _OperandsDatas[op + 1].Position - _OperandsDatas[op].Position - 1;;
            }
            for(int i = 0; i < input.Count - 1; i ++) {
                var startFrom = _OperandsDatas[op].Position;
                var s = input[i].Substring(startFrom, spacing);
                _NumbersString[i].Add(s);
            }
        }
    }

    #endregion

    #region Private

    private readonly Dictionary<int, List<string>> _NumbersString = [];
    private readonly List<OperandDefinition> _OperandsDatas = [];
    private record OperandDefinition(char Operand, int Position);

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

    private static ulong PerformOperationAdvanced(List<string> numbers, char operand) {
        ulong retValue = (ulong) (operand == '+' ? 0 : 1);

        //  Read columns
        var board = CreateMatrix(numbers, operand, numbers.First().Length);
        for(int i = 0; i < board.X; i ++) {
            switch(operand) {
                case '+':
                    retValue += ReadColumn(board, i);
                    break;
                case '*':
                    retValue *= ReadColumn(board, i);
                    break;
            }
        }

        return retValue;
    }

    private static Board<char> CreateMatrix(List<string> numbers, char operand, int length) {
        Board<char> board = new(length, numbers.Count);

        int index = 0;
        for(int rowIndex = 0; rowIndex < numbers.Count; rowIndex ++) {
            for(int c = 0; c < numbers[rowIndex].Length; c ++) {
                board.SetValue(index, numbers[rowIndex][c]);
                index ++;
            }
        }

        return board;
    }

    private static ulong ReadColumn(Board<char> board, int column) {
        var strValue = string.Empty;
        for(int row = 0; row < board.Y; row ++) {
            var strColumn = board[column, row].ToString();
            strValue += strColumn;
        }
        return ulong.Parse(strValue);
    }

    #endregion

}