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
            reset += clicks;
        });
         return reset;
    }

    #region Private

    private const int _Start = 50;
    private const int _Max = 100;

    private (int, int) Rotation(int start, string direction, int steps) {
        int multiplier = direction.ToLower() switch {
            "l" => -1,
            "r" => 1,
            _ => throw new ArgumentException("Invalid direction")
        };
        var resultPartial =  start + (multiplier * steps);
        var result = resultPartial % _Max;
        var resetDuringRotation = 0;
        
        if(result < 0 || result >= _Max) {
            // Adjust for negative modulo results 
            result += multiplier * -1 * _Max;
        }

        int noOfClicks = 0;
        if(resultPartial < 0) {
            noOfClicks = (Math.Abs(resultPartial) / _Max) + (start == 0 ? 0 : 1);
        } else if(resultPartial >= _Max) {
            noOfClicks = resultPartial / _Max;
        } else if(resultPartial == 0) {
            noOfClicks = 1;
        }
        resetDuringRotation += noOfClicks;

        return (result, resetDuringRotation);
    }

    #endregion

}