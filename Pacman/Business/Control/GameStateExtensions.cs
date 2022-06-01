using System.Text;
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
            Direction.North => new Coordinate(x, Utilities.Mod(y - 1, width)),
            Direction.South => new Coordinate(x, Utilities.Mod(y + 1, width)),
            Direction.East => new Coordinate(Utilities.Mod(x + 1, length), y),
            Direction.West => new Coordinate(Utilities.Mod(x - 1, length), y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return obstacles.All(o => o.Coordinate != newCoord) ? newCoord : coordinate;
    }

    public static string GetString(this GameState gameState)
    {
        var (width, length) = gameState.Size;
        var res = new StringBuilder(new string(Constants.Blank, length * width));
        var entities = gameState.Pellets.Values.
            Concat(gameState.Walls).
            Append(gameState.Pac).
            Concat(gameState.Ghosts).
            OrderBy(o => o.Coordinate.Y).
            ThenBy(o => o.Coordinate.X).
            ToList();

        foreach (var entity in entities)
            res[entity.Coordinate.GetRank(width)] = entity.Symbol;

        for (var l = 0; l < length; l++)
            res.Insert(width * (l + 1) + l, '\n');

        return res.ToString();
    }

    public static IEnumerable<MovableEntity> GetMovableEntities(this GameState gameState) =>
       new MovableEntity[] {gameState.Pac}.Concat(gameState.Ghosts);
}
