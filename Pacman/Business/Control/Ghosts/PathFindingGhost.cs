using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public record PathFindingGhost(Coordinate Coordinate, int Id) : BaseGhost(Coordinate, Constants.PathFindingGhost, Id)
{
    public override GameState Move(GameState gameState)
    {
        // var ghostCoord = gameState.GetMovableEntities().ElementAt(Id).Coordinate;
        if (gameState.Pac.Coordinate == Coordinate) return gameState;

        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
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
                lastDequeuedCoord = dequeuedCoords.Any() ? dequeuedCoords.Last() : Coordinate;
            }
            catch (InvalidOperationException)
            {
                return gameState;
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var currCoord = gameState.GetNewCoord(lastDequeuedCoord, direction, obstacles);
                
                if (currCoord == lastDequeuedCoord || visitedCoords.Contains(currCoord)) continue;

                visitedCoords.Add(currCoord);
                
                possiblePaths.Enqueue(dequeuedCoords.Length < 2
                    ? dequeuedCoords.Append(currCoord).ToArray()
                    : new[] {dequeuedCoords.First(), currCoord});
            }

        } while (lastDequeuedCoord != gameState.Pac.Coordinate);

        return gameState.UpdateGhostCoordinate(Id, dequeuedCoords.First());
    }
}
