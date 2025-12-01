using System.Net.WebSockets;

namespace AdventOfCode_2025.Days;

internal class Day01 : Day {

    public override object Basic() {
        int current = _Start;
        int reset = 0;
        _Input.ForEach(line => {
            (current, _) = Rotation(current, line[0].ToString(), int.Parse(line[1..]));
            if(current == 0) {
                reset ++;
            }
        });
        return reset;
    }

    public override object Advanced() {
        int current = _Start;
        int reset = 0;
        int clicks = 0;
        _Input.ForEach(line => {
            (current, clicks) = Rotation(current, line[0].ToString(), int.Parse(line[1..]));
            if(current == 0) {
                reset ++;
            }
            reset += clicks;
        });
        return reset;
    }

    #region Private

    private readonly int _Start = 50;
    private readonly int _Max = 100;

    private (int, int) Rotation(int current, string direction, int steps) {
        int multiplier = direction.ToLower() switch {
            "l" => -1,
            "r" => 1,
            _ => throw new ArgumentException("Invalid direction")
        };
        var resultPartial =  current + (multiplier * steps);
        var resetDuringRotation = 0;
        var result = resultPartial % _Max;
        if(result < 0 || result >= _Max) {
            result += multiplier * _Max * -1;
        }

        if(current != 0 && result != 0) {
            int noOfClicks = 0;
            if(resultPartial < 0) {
                noOfClicks = (Math.Abs(resultPartial) / _Max) + 1;
            } else if(resultPartial >= _Max) {
                noOfClicks = resultPartial / _Max;
            }
            resetDuringRotation = noOfClicks;
        }

        Console.WriteLine($"{current} : {direction}{steps} --> {result} (reset: {result == 0}, clicks: {resetDuringRotation})");
        
        return (result, resetDuringRotation);
    }

    #endregion

}