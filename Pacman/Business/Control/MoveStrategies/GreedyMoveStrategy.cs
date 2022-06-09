using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class GreedyMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var coordinate = movableEntity.Coordinate;
        if (gameState.Pac.Coordinate == coordinate) return coordinate;
        var bestDistance = gameState.Pac.Coordinate.GetDistance(coordinate);
        var bestCoord = coordinate;
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GetNewCoord(coordinate, direction, obstaclesArr);
            var currentDistance = gameState.Pac.Coordinate.GetDistance(currentCoord);
            
            if (currentDistance >= bestDistance) continue;
            
            bestDistance = currentDistance;
            bestCoord = currentCoord;
        }

        return bestCoord;
    }
}