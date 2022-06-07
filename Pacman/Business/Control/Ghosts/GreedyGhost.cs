using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public record GreedyGhost(Coordinate Coordinate, int Id) : BaseGhost(Coordinate, Constants.GreedyGhost, Id)
{
    public override GameState Move(GameState gameState)
    {
        if (gameState.Pac.Coordinate == Coordinate) return gameState;
        
        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
        var bestDistance = gameState.Pac.Coordinate.GetDistance(Coordinate);
        var bestCoord = Coordinate;
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GetNewCoord(Coordinate, direction, obstacles);
            var currentDistance = gameState.Pac.Coordinate.GetDistance(currentCoord);
            
            if (currentDistance >= bestDistance) continue;
            
            bestDistance = currentDistance;
            bestCoord = currentCoord;
        }

        return gameState.UpdateGhostCoordinate(Id, bestCoord);
    }
}
