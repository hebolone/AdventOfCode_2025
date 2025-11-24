namespace AdventOfCode.Tools;

internal record Coordinate(int X, int Y) {
    public override string ToString() => $"({X},{Y})";
	public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
  	public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);
	public static Coordinate operator +(Coordinate a, (int x, int y) b) => new(a.X + b.x, a.Y + b.y);
}

internal class Board<T> : IEnumerable<T> {

    private readonly int _X;
    private readonly int _Y;
    private readonly List<T> _Cells = [];
    private readonly Func<T, string> _PrintFunc = (i) => i?.ToString() ?? ".";

    public Board(int x, int y, Func<T>? initializer = null) {
        _X = x;
        _Y = y;
        for(int i = 0; i < (_X * _Y); i ++) {
            T? t = (initializer == null ? default : initializer()) ?? throw new ArgumentNullException($"Argument of type {typeof(T)} must have an explicit initializer.");
            _Cells.Add(t);
        } 
    }

    #region Properties

    public int Size => _X * _Y;
    public int X => _X;
    public int Y => _Y;
    public T this[int i] => _Cells[i];
    public T this[int xFrom, int yFrom] => this[xFrom + yFrom * _X];
    public T this[Coordinate coordinate] => this[ConvertCoordinatesToIndex(coordinate)];
    public void SetValue(int xFrom, int yFrom, T t) => SetValue(xFrom + yFrom * _X, t);
    public void SetValue(int index, T t) => _Cells[index] = t;
    public void SetValue(Coordinate coordinate, T t) => SetValue(coordinate.X, coordinate.Y, t);
    public List<Coordinate> GetSurrounding(int index) {
        var actual = ConvertIndexToCoordinates(index);
        List<Coordinate> retValue = [];
        List<Coordinate> surroundingCells = [
            new (actual.X - 1, actual.Y - 1),
            new (actual.X, actual.Y - 1),
            new (actual.X + 1, actual.Y - 1),
            new (actual.X - 1, actual.Y),
            new (actual.X + 1, actual.Y),
            new (actual.X - 1, actual.Y + 1),
            new (actual.X, actual.Y + 1),
            new (actual.X + 1, actual.Y + 1)
        ];
        surroundingCells.ForEach(c => {
            if(!IsOutside(c)) {
                retValue.Add(c);
            }
        });
        return retValue;
    }

    public List<Coordinate> GetCrossSurrounding(int index) {
        var actual = ConvertIndexToCoordinates(index);
        List<Coordinate> retValue = [];
        List<Coordinate> surroundingCells = [
            new (actual.X, actual.Y - 1),
            new (actual.X - 1, actual.Y),
            new (actual.X + 1, actual.Y),
            new (actual.X, actual.Y + 1),
        ];
        surroundingCells.ForEach(c => {
            if(!IsOutside(c)) {
                retValue.Add(c);
            }
        });
        return retValue;
    }

    public List<Coordinate> GetSurrounding(Coordinate coordinate) => GetSurrounding(ConvertCoordinatesToIndex(coordinate));
    
    public List<Coordinate> GetCrossSurrounding(Coordinate coordinate) => GetCrossSurrounding(ConvertCoordinatesToIndex(coordinate));

    #endregion

    #region Public

    public string PrintBoard(Func<T, string>? printFunc = null) {
        var sb = new StringBuilder();
        for(int y = 0; y < _Y; y ++) {
            for(int x = 0; x < _X; x ++) {
                var printer = printFunc ?? _PrintFunc;
                sb.Append(printer(this[x, y]));
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public Coordinate ConvertIndexToCoordinates(int index) {
        var y_derived = index / _Y;
        var x_derived = index - (_Y * y_derived);
        return new(x_derived, y_derived); 
    }

    public int ConvertCoordinatesToIndex(Coordinate coordinate) => coordinate.X + coordinate.Y * _X;

    public bool IsOutside(Coordinate coordinate) => coordinate.X < 0 || coordinate.Y < 0 || coordinate.X > _X - 1 || coordinate.Y > _Y - 1;
    public bool IsInside(Coordinate coordinate) => !IsOutside(coordinate);

    #endregion

    #region Private

    #endregion

    #region IEnumerable

    public IEnumerator<T> GetEnumerator() => _Cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    #endregion

}