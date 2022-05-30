using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class PathFindingGhost : MovableEntity
{
    private readonly Pac _pac;

    public PathFindingGhost(Coordinate coordinate, Pac pac) : base(coordinate)
    {
        _pac = pac;
    }

    public override void Move(GameState gameState)
    {
        if (_pac.Coordinate == Coordinate) return;
        
        var obstacles = gameState.Walls.Concat(gameState.Ghosts).ToArray();
        var possiblePaths = new Queue<IEnumerable<Coordinate>>();
        possiblePaths.Enqueue(new List<Coordinate> {Coordinate});
        
        Coordinate lastDequeuedCoord;
        IEnumerable<Coordinate> dequeuedCoords;

        do
        {
            dequeuedCoords = possiblePaths.Dequeue().ToArray();
            lastDequeuedCoord = dequeuedCoords.Last();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var currCoord = gameState.GetNewCoord(lastDequeuedCoord, direction, obstacles);
                
                if (currCoord == lastDequeuedCoord) continue;
                possiblePaths.Enqueue(dequeuedCoords.Append(currCoord));
            }
            
        } while (lastDequeuedCoord != _pac.Coordinate);

        Coordinate = dequeuedCoords.Skip(1).First();
    }
}
