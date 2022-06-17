using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class GreedyMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        if (gameState.Pac.Coordinate == startingCoord) return startingCoord;
        var bestDistance = gameState.Pac.Coordinate.GetDistance(startingCoord);
        var bestCoord = startingCoord;
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GameStateExtensions(startingCoord, direction, obstaclesArr);
            var currentDistance = gameState.Pac.Coordinate.GetDistance(currentCoord);
            
            if (currentDistance >= bestDistance) continue;
            
            bestDistance = currentDistance;
            bestCoord = currentCoord;
        }

        return bestCoord;
    }
}
