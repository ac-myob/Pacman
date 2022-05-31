using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class PathFindingGhost : MovableEntity
{
    private readonly Pac _pac;

    public PathFindingGhost(Coordinate coordinate, Pac pac) : base(coordinate, Constants.PathFindingGhost)
    {
        _pac = pac;
    }

    public override void Move(GameState gameState)
    {
        if (_pac.Coordinate == Coordinate) return;
        
        var obstacles = gameState.Walls.Concat(gameState.Ghosts).ToArray();
        var possiblePaths = new Queue<Coordinate[]>();
        possiblePaths.Enqueue(Array.Empty<Coordinate>());
        
        Coordinate[] dequeuedCoords;
        Coordinate lastDequeuedCoord;
        
        do
        {
            // If cannot move, do nothing
            try
            {
                dequeuedCoords = possiblePaths.Dequeue().ToArray();
                lastDequeuedCoord = dequeuedCoords.Any() ? dequeuedCoords.Last() : Coordinate;
            }
            catch (InvalidOperationException)
            {
                return;
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var currCoord = gameState.GetNewCoord(lastDequeuedCoord, direction, obstacles);
                if (currCoord == lastDequeuedCoord) continue;

                possiblePaths.Enqueue(dequeuedCoords.Length < 2
                    ? dequeuedCoords.Append(currCoord).ToArray()
                    : new[] {dequeuedCoords.First(), currCoord});
            }
            
        } while (lastDequeuedCoord != _pac.Coordinate);

        // Select second coordinate because first will always be ghost's starting coordinate
        Coordinate = dequeuedCoords.First();
    }
}
