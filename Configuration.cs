internal class Settings {
    public int? Year { get; set; } = DateTime.Now.Year;
    public string InputFilesDir { get; set; } = string.Empty;
}

internal class Configuration {
    public Settings Settings { get; set; } = new();
}