using System.Net.WebSockets;

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
        int current = _Start;
        int clicks = 0;
        _Input.ForEach(line => {
            (current, clicks) = RotationAdvanced(current, line[0].ToString(), int.Parse(line[1..]));
            if(current == 0) {
                _Reset++;
            }
            _Reset += clicks;
        });
        return _Reset;
    }

    #region Protected

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

    private (int, int) RotationAdvanced(int current, string direction, int steps) {
        int multiplier = direction.ToLower() switch {
            "l" => -1,
            "r" => 1,
            _ => throw new ArgumentException("Invalid direction")
        };
        var resultPartial =  current + (multiplier * steps);
        var resetDuringRotation = 0;
        if(resultPartial < 0 || resultPartial >= _Max) {
            int noOfClicks = resultPartial / 100;
            resultPartial = noOfClicks;            
        }
        var result = resultPartial % _Max;
        if(result < 0 || result >= _Max) {
            result += _Max;
        }
        Console.WriteLine($"{current} : {direction}{steps} --> {result} (resets: {resetDuringRotation})");
        return (result, resetDuringRotation);
    }

    #endregion

}