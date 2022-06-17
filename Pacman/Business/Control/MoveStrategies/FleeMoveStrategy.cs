using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class FleeMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var bestCoord = startingCoord;
        var bestDistance = startingCoord.GetDistance(gameState.Pac.Coordinate);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GetNewCoord(startingCoord, direction, obstaclesArr);
            var currentDistance = currentCoord.GetDistance(gameState.Pac.Coordinate);

            bestCoord = currentDistance > bestDistance ? currentCoord : bestCoord;
        }

        return bestCoord;
    }
}
