namespace Pacman;

public class Pac
{
    public Coordinate Coordinate { get; private set; }
    private readonly IReader _reader;
    private readonly IWriter _writer;

    public Pac(Coordinate coordinate, IReader reader, IWriter writer)
    {
        Coordinate = coordinate;
        _reader = reader;
        _writer = writer;
    }
    public void Move(Size size)
    {
        var (x, y) = Coordinate;

        int Mod(int x, int m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        Coordinate = _reader.ReadKey() switch
        {
            "UpArrow" => new Coordinate(x, Mod(y - 1, size.Length)),
            "DownArrow" => new Coordinate(x, Mod(y + 1, size.Length)),
            "LeftArrow" => new Coordinate(Mod(x - 1, size.Width), y),
            "RightArrow" => new Coordinate(Mod(x + 1, size.Width), y),
            _ => Coordinate
        };
    }
}