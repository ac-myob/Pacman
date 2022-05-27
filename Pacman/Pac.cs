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

        int Mod(int num, int m)
        {
            var r = num % m;
            return r < 0 ? r + m : r;
        }

        var keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
        
        Coordinate = keyPress switch
        {
            Constants.UpKey => new Coordinate(x, Mod(y - 1, size.Length)),
            Constants.DownKey => new Coordinate(x, Mod(y + 1, size.Length)),
            Constants.LeftKey => new Coordinate(Mod(x - 1, size.Width), y),
            Constants.RightKey => new Coordinate(Mod(x + 1, size.Width), y),
            _ => Coordinate
        };
    }
}