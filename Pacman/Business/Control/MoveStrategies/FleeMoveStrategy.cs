using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class FleeMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, Func<Coordinate, bool> isBlocked, GameState gameState)
    {
        if (gameState.Pac.Coordinate == startingCoord) return startingCoord;
        
        var bestCoord = startingCoord;
        var bestDistance = gameState.Pac.Coordinate.GetDistance(startingCoord);
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = startingCoord.Shift(direction, gameState.Size);

            if (isBlocked(currentCoord)) continue;

            var currentDistance = gameState.Pac.Coordinate.GetDistance(currentCoord);
            
            if (currentDistance <= bestDistance) continue;
            
            bestDistance = currentDistance;
            bestCoord = currentCoord;
        }

        return bestCoord;
    }
}
