using System.Collections;

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
    public void Move(Size size, IEnumerable<Wall> walls)
    {
        var (x, y) = Coordinate;
        var (width, length) = size;
        var wallsArr = walls.ToArray();

        Coordinate newCoord;
        bool validNewCoord;
        
        do
        {
            var keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
            newCoord = keyPress switch
            {
                Constants.UpKey => new Coordinate(x, Utilities.Mod(y - 1, length)),
                Constants.DownKey => new Coordinate(x, Utilities.Mod(y + 1, length)),
                Constants.LeftKey => new Coordinate(Utilities.Mod(x - 1, width), y),
                Constants.RightKey => new Coordinate(Utilities.Mod(x + 1, width), y),
                _ => Coordinate
            };

            validNewCoord = wallsArr.All(w => w.Coordinate != newCoord);
            
            if (!validNewCoord) _writer.WriteLine(Messages.WallObstruction);

        } while (!validNewCoord);

        Coordinate = newCoord;
    }
}
