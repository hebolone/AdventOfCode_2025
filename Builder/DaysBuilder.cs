using System.Diagnostics;

namespace AdventOfCode.Builder;

internal class DaysBuilder(string basePath) : IDaysBuilder {

    private readonly string _BasePath = basePath;
    private readonly List<int> _TestDays = [];
    private readonly Dictionary<int, Day> _Days = [];

    #region IDaysBuilder

    public IDaysBuilder AddDay(int id, Day day) {
        var result = _Days.TryAdd(id, day);
        if(!result) {
            throw new ArgumentException($"A solver for day '{id}' is already registered.");
        }
        return this;
    }

    public IDaysBuilder Solve(int? id = null, TSolveType solveType = TSolveType.BOTH) {
        var idToSolve = id == null ? _Days.Keys.Max() : (int) id;
        var dayFound = _Days.ContainsKey(idToSolve);

        if(dayFound) {
            //  Read input file
            var solver = _Days[idToSolve];
            var isTest = _TestDays.Contains(idToSolve);
            solver.IsTest = isTest;
            solver.SetInput(ReadInput(idToSolve, isTest));

            //  Get solution
            var results = new List<Result>();
            switch(solveType) {
                case TSolveType.BASIC:
                    var (resultBasic, elapsedMillisecondsBasic) = Execution(solver.Basic);
                    results.Add(new(idToSolve, resultBasic, solveType, isTest, elapsedMillisecondsBasic));
                    break;
                case TSolveType.ADVANCED:
                    var (resultAdvanced, elapsedMillisecondsAdvanced) = Execution(solver.Advanced);
                    results.Add(new(idToSolve, resultAdvanced, solveType, isTest, elapsedMillisecondsAdvanced));
                    break;
                default:
                    var (resultBothBasic, elapsedMillisecondsBothBasic) = Execution(solver.Basic);
                    var (resultBothAdvanced, elapsedMillisecondsBothAdvanced) = Execution(solver.Advanced);
                    results.Add(new(idToSolve, resultBothBasic, TSolveType.BASIC, isTest, elapsedMillisecondsBothBasic));
                    results.Add(new(idToSolve, resultBothAdvanced, TSolveType.ADVANCED, isTest, elapsedMillisecondsBothAdvanced));
                    break;
            }

            results.ForEach(r => {
                Console.WriteLine(r.ToString());
            });
        } else {
            var message = $"Day '{idToSolve}' is not present on list.";
            throw new ArgumentException(message);
        }

        return this;

    }

    public IDaysBuilder SetTests(params int [] ids) {
        if(ids.Length == 0) {
            _TestDays.Add(_Days.Max(d => d.Key));
        } else {
            foreach(var i in ids) {
                _TestDays.Add(i);
            }
        }

        return this;
    }

    #endregion

    #region Private

    private List<string> ReadInput(int id, bool isTest) {
        //  Check if file exists
        var inputFilePath = GetInputFileName(id, isTest);
        if(!File.Exists(inputFilePath)) {
            throw new FileNotFoundException($"Missing input file: '{inputFilePath}'. Contact Sm3P");
        }

        var retValue = new List<string>();
        var lines = File.ReadLines(inputFilePath);
        
        foreach(var line in lines) {
            retValue.Add(line);
        }

       return retValue;
    }

    private string GetInputFileName(int id, bool isTest) => Path.Combine(_BasePath, $"day_{id:00.##}{(isTest ? "_test" : "")}.txt");
    
    private static (object result, long elapsedMilliseconds) Execution(Func<object> func) {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = func();
        stopWatch.Stop();
        return (result, stopWatch.ElapsedMilliseconds);
    }

    #endregion

}