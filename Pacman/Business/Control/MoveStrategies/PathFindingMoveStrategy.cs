using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class PathFindingMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, IEnumerable<Entity> obstacles, GameState gameState)
    {
        if (gameState.Pac.Coordinate == startingCoord) return startingCoord;
        
        var possiblePaths = new Queue<Coordinate[]>();
        possiblePaths.Enqueue(Array.Empty<Coordinate>());
        var obstaclesArr = obstacles.ToArray();
        Coordinate[] dequeuedCoords;
        Coordinate lastDequeuedCoord;
        var visitedCoords = new HashSet<Coordinate>();
        
        do
        {
            // If cannot move, do nothing
            try
            {
                dequeuedCoords = possiblePaths.Dequeue().ToArray();
                lastDequeuedCoord = dequeuedCoords.Any() ? dequeuedCoords.Last() : startingCoord;
            }
            catch (InvalidOperationException)
            {
                return startingCoord;
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var currCoord = gameState.GameStateExtensions(lastDequeuedCoord, direction, obstaclesArr);
                
                if (currCoord == lastDequeuedCoord || visitedCoords.Contains(currCoord)) continue;

                visitedCoords.Add(currCoord);
                
                possiblePaths.Enqueue(dequeuedCoords.Length < 2
                    ? dequeuedCoords.Append(currCoord).ToArray()
                    : new[] {dequeuedCoords.First(), currCoord});
            }

        } while (lastDequeuedCoord != gameState.Pac.Coordinate);

        return dequeuedCoords.First();
    }
}
