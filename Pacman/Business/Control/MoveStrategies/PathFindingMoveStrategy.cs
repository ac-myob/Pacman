using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class PathFindingMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, Func<Coordinate, bool> isBlocked, GameState gameState)
    {
        if (gameState.Pac.Coordinate == startingCoord) return startingCoord;
        
        var possiblePaths = new Queue<Coordinate[]>();
        possiblePaths.Enqueue(Array.Empty<Coordinate>());
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
                var currentCoord = lastDequeuedCoord.Shift(direction, gameState.Size);

                if (isBlocked(currentCoord) || visitedCoords.Contains(currentCoord)) continue;

                visitedCoords.Add(currentCoord);
                
                possiblePaths.Enqueue(dequeuedCoords.Length < 2
                    ? dequeuedCoords.Append(currentCoord).ToArray()
                    : new[] {dequeuedCoords.First(), currentCoord});
            }

        } while (lastDequeuedCoord != gameState.Pac.Coordinate);

        return dequeuedCoords.First();
    }
}
