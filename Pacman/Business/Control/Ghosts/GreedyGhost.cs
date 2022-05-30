using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class GreedyGhost : MovableEntity
{
    private readonly Pac _pac;

    public GreedyGhost(Coordinate coordinate, Pac pac) : base(coordinate)
    {
        _pac = pac;
    }

    public override void Move(GameState gameState)
    {
        if (_pac.Coordinate == Coordinate) return;
        
        var obstacles = gameState.Walls.Concat(gameState.Ghosts).ToArray();
        var bestDistance = double.MaxValue;
        var bestCoord = Coordinate;
        
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var currentCoord = gameState.GetNewCoord(Coordinate, direction, obstacles);
            var currentDistance = _distanceFromPac(currentCoord);
            
            if (currentDistance >= bestDistance) continue;
            
            bestDistance = currentDistance;
            bestCoord = currentCoord;
        }

        Coordinate = bestCoord;
    }

    private double _distanceFromPac(Coordinate coordinate)
    {
        var (x1, y1) = coordinate;
        var (x2, y2) = _pac.Coordinate;

        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }
}
