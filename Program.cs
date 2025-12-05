using var reader = new StreamReader("appsettings.json");
var configuration = JsonSerializer.Deserialize<Configuration>(reader.BaseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
var year = configuration?.Settings.Year;
var inputDir = configuration!.Settings.InputFilesDir.Replace("%1%", year.ToString());
var daysBuilder = new DaysBuilder(basePath: inputDir);

daysBuilder
    //.AddDay(1, new Day01())
    //.AddDay(2, new Day02())
    //.AddDay(3, new Day03())
    //.AddDay(4, new Day04())
    .AddDay(5, new Day05())
    .SetTests()
    //.Solve();
  //  .Solve(solveType: TSolveType.BASIC);    
    .Solve(solveType: TSolveType.ADVANCED);