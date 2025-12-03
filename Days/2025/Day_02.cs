namespace AdventOfCode_2025.Days;

internal class Day02 : Day {

    public override object Basic() {
        List<long> invalidIDs = [];
        _Ranges.ForEach(r => {
            var invalidIDsInRange = CheckRange(r, IsInvalid);
            invalidIDs.AddRange(invalidIDsInRange);
        });

        return invalidIDs.Sum();
    }

    public override object Advanced() {
        List<long> invalidIDs = [];
        _Ranges.ForEach(r =>{
            var invalidIDsInRange = CheckRange(r, IsInvalidAdvanced);
            invalidIDs.AddRange(invalidIDsInRange);
        });

        return invalidIDs.Sum();
    }

    #region Protected

    protected override void Parse(List<string> input) {
        var parts = input.First().Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach(var part in parts) {
            var item = part.Split('-');
            TRange range = new(long.Parse(item[0]), long.Parse(item[1]));
            _Ranges.Add(range);
        }
    }

    #endregion

    #region Private

    private record TRange(long Start, long End);
    private List<TRange> _Ranges = [];
    private delegate bool HandleIsInvalid(long i);


    private static List<long> CheckRange(TRange range, HandleIsInvalid checker) {
        List<long> retValue = [];
        for(long i = range.Start; i <= range.End; i++) {
            if(i > 9 && checker(i)) {
                retValue.Add(i);
            }
        }
        return retValue;
    }

    private static bool IsInvalid(long i) {
        string s = i.ToString();
        int half = s.Length / 2;
        string s1 = s[..half];
        string s2 = s[half..];
        return s1 == s2;
    }

    private static bool IsInvalidAdvanced(long i) {
        string s = i.ToString();
        var length = s.Length;
        var divisors = FindDivisors(length);
        foreach(var divisor in divisors) {
            var builtNumber = string.Concat(Enumerable.Repeat(s[..divisor], length / divisor));
            if(builtNumber == s) {
                return true;
            }
        }
        return false;
    }

    public static List<int> FindDivisors(int number) {
        List<int> divisors = [];
        
        divisors.Add(1);

        int limit = (int)Math.Sqrt(number);

        for (int i = 2; i <= limit; i++) {
            if (number % i == 0) {
                divisors.Add(i);

                int divisorCouple = number / i;

                if (i != divisorCouple) {
                    divisors.Add(divisorCouple);
                }
            }
        }

        divisors.Sort();
        
        return divisors;
    }

    #endregion

}