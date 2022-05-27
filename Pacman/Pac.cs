namespace Pacman;

public class Pac
{
    public Coordinate Coordinate { get; private set; }
    private readonly Query _query;
    private readonly IWriter _writer;

    public Pac(Coordinate coordinate, IReader reader, IWriter writer)
    {
        Coordinate = coordinate;
        _query = new Query(reader, writer);
        _writer = writer;
    }
    public void Move(Size size)
    {
        var (x, y) = Coordinate;
        var (width, length) = size;

        var keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
        
        Coordinate = keyPress switch
        {
            Constants.UpKey => new Coordinate(x, Utilities.Mod(y - 1, length)),
            Constants.DownKey => new Coordinate(x, Utilities.Mod(y + 1, length)),
            Constants.LeftKey => new Coordinate(Utilities.Mod(x - 1, width), y),
            Constants.RightKey => new Coordinate(Utilities.Mod(x + 1, width), y),
            _ => Coordinate
        };
    }
}