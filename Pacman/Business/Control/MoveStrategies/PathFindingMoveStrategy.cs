using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class PathFindingMoveStrategy : IMoveStrategy
{
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var coordinate = movableEntity.Coordinate;
        if (gameState.Pac.Coordinate == coordinate) return coordinate;
        
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
                lastDequeuedCoord = dequeuedCoords.Any() ? dequeuedCoords.Last() : coordinate;
            }
            catch (InvalidOperationException)
            {
                return coordinate;
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var currCoord = gameState.GetNewCoord(lastDequeuedCoord, direction, obstaclesArr);
                
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