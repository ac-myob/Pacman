using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public static class GameStateExtensions
{
    public static Coordinate GetNewCoord(
        this GameState gameState, Coordinate coordinate, Direction direction, IEnumerable<Entity> obstacles)
    {
        var (x, y) = coordinate;
        var (length, width) = gameState.Size;
        var newCoord = direction switch
        {
            Direction.North => new Coordinate(x, Utilities.Mod(y - 1, length)),
            Direction.South => new Coordinate(x, Utilities.Mod(y + 1, length)),
            Direction.East => new Coordinate(Utilities.Mod(x + 1, width), y),
            Direction.West => new Coordinate(Utilities.Mod(x - 1, width), y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return obstacles.All(o => o.Coordinate != newCoord) ? newCoord : coordinate;
    }
}
