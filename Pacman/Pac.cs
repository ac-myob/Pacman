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
    public void Move()
    {
        var (x, y) = Coordinate;

        Coordinate = _reader.ReadKey() switch
        {
            "UpArrow" => new Coordinate(x, y - 1),
            "DownArrow" => new Coordinate(x, y + 1),
            "LeftArrow" => new Coordinate(x - 1, y),
            "RightArrow" => new Coordinate(x + 1, y),
            _ => Coordinate
        };
    }
}