using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public static class GhostFactory
{ 
    public static BaseGhost GetGhost(GhostType ghostType, Coordinate coordinate, int id)
    {
        return ghostType switch
        {
            GhostType.Random => new RandomGhost(coordinate, id, new RandomSelector<Coordinate>()),
            GhostType.Greedy => new GreedyGhost(coordinate, id),
            GhostType.PathFinding => new PathFindingGhost(coordinate, id),
            _ => throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null)
        };
    }
}