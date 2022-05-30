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
            Direction.North => new Coordinate(x, Utilities.Mod(y - 1, length)),
            Direction.South => new Coordinate(x, Utilities.Mod(y + 1, length)),
            Direction.East => new Coordinate(Utilities.Mod(x + 1, width), y),
            Direction.West => new Coordinate(Utilities.Mod(x - 1, width), y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return obstacles.All(o => o.Coordinate != newCoord) ? newCoord : coordinate;
    }

    public static string GetString(this GameState gameState)
    {
        var res = new StringBuilder();
        var entities = gameState.Ghosts.
            Append(gameState.Pac).
            Concat(gameState.Walls).
            Concat(gameState.Pellets).
            OrderBy(o => o.Coordinate.Y).
            ThenBy(o => o.Coordinate.X).
            ToList();
        var entityIndex = 0;
    
        for (var l = 0; l < gameState.Size.Length; l++)
        {
            for (var w = 0; w < gameState.Size.Width; w++)
            {
                var nextSymbol = Constants.Blank;
                if (entityIndex < entities.Count)
                {
                    var currentEntity = entities[entityIndex];

                    while (!(currentEntity.Coordinate.Y > l || 
                             currentEntity.Coordinate.X >= w && 
                             currentEntity.Coordinate.Y == l))
                    {
                        ++entityIndex;
                        currentEntity = entities[entityIndex];
                    }

                    if (currentEntity.Coordinate == new Coordinate(w, l))
                        nextSymbol = currentEntity.Symbol;
                }
                res.Append(nextSymbol);
            }
            res.Append('\n');
        }
        
        return res.ToString();
    }
}
