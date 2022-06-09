using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class FleeMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var coordinate = movableEntity.Coordinate;
        var bestCoord = coordinate;
        var bestDistance = coordinate.GetDistance(gameState.Pac.Coordinate);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GetNewCoord(coordinate, direction, obstaclesArr);
            var currentDistance = currentCoord.GetDistance(gameState.Pac.Coordinate);

            bestCoord = currentDistance > bestDistance ? currentCoord : bestCoord;
        }

        return bestCoord;
    }
}