using System.Text;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public static class GameStateQuery
{
    public static Coordinate GameStateExtensions(
        this GameState gameState, Coordinate coordinate, Direction direction, IEnumerable<Entity> obstacles)
    {
        var (x, y) = coordinate;
        var (length, width) = gameState.Size;
        var newCoord = direction switch
        {
            Direction.North => coordinate with{Y = Utilities.Mod(y - 1, width)},
            Direction.South => coordinate with{Y = Utilities.Mod(y + 1, width)},
            Direction.East => coordinate with{X = Utilities.Mod(x + 1, length)},
            Direction.West => coordinate with{X = Utilities.Mod(x - 1, length)},
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return obstacles.All(o => o.Coordinate != newCoord) ? newCoord : coordinate;
    }

    public static string GetString(this GameState gameState)
    {
        var (width, length) = gameState.Size;
        var res = new StringBuilder(new string(Constants.Blank, length * width));
        var entities = gameState.GetPellets().Cast<Entity>().
            Concat(gameState.Walls).
            Concat(gameState.GetMovableEntities()).
            OrderBy(o => o.Coordinate.Y).
            ThenBy(o => o.Coordinate.X).
            ToArray();

        foreach (var entity in entities)
            res[entity.Coordinate.GetRank(width)] = entity.Symbol;

        for (var l = 0; l < length; l++)
            res.Insert(width * (l + 1) + l, Environment.NewLine);

        return res.ToString();
    }
    
    public static IEnumerable<Pellet> GetPellets(this GameState gameState) =>
        gameState.Pellets.Where(p => !p.Eaten);
    
    public static IEnumerable<MovableEntity> GetMovableEntities(this GameState gameState) => 
        gameState.Ghosts.Cast<MovableEntity>().Prepend(gameState.Pac);

    public static bool IsDirectionValid(this GameState gameState, Direction direction)
    {
        var currentPacCoord = gameState.Pac.Coordinate;
        var newPacCoord = gameState.GameStateExtensions(currentPacCoord, direction, gameState.Walls);

        return currentPacCoord != newPacCoord;
    }

    public static bool IsGameFinished(this GameState gameState) => 
        gameState.Round > Constants.MaxRounds || gameState.Lives == 0;
}
