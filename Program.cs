using var reader = new StreamReader("appsettings.json");
var configuration = JsonSerializer.Deserialize<Configuration>(reader.BaseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
var daysBuilder = new DaysBuilder(basePath: configuration!.Settings.InputFilesDir);

daysBuilder
    .AddDay(1, new Day01())
    //.SetTests()
//    .Solve();
    .Solve(solveType: TSolveType.BASIC);    
//    .Solve(solveType: TSolveType.ADVANCED);