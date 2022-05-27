using Pacman.Business.Control;
using Pacman.Variables;

namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity
{
    protected MovableEntity(Coordinate coordinate) : base(coordinate) { }

    public abstract void Move(GameState gameState);

    protected Coordinate GetNewCoord(Direction direction, Size size, IEnumerable<Entity> obstacles)
    {
        var (x, y) = Coordinate;
        var (length, width) = size;
        var newCoord = direction switch
        {
            Direction.North => new Coordinate(x, Utilities.Mod(y - 1, length)),
            Direction.South => new Coordinate(x, Utilities.Mod(y + 1, length)),
            Direction.East => new Coordinate(Utilities.Mod(x - 1, width), y),
            Direction.West => new Coordinate(Utilities.Mod(x + 1, width), y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return obstacles.All(o => o.Coordinate != newCoord) ? newCoord : Coordinate;
    }
}
