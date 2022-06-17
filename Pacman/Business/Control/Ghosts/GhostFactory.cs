using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class GhostFactory
{
    private readonly ISelector<Coordinate> _selector;

    public GhostFactory(ISelector<Coordinate> selector)
    {
        _selector = selector;
    }

    public Ghost GetGhost(GhostType ghostType, Coordinate coordinate)
    {
        IMoveStrategy moveStrategy;
        char symbol;

        switch (ghostType)
        {
            case GhostType.Random:
                moveStrategy = new RandomMoveStrategy(_selector);
                symbol = Constants.RandomGhost;
                break;
            case GhostType.Greedy:
                moveStrategy = new GreedyMoveStrategy();
                symbol = Constants.GreedyGhost;
                break;
            case GhostType.PathFinding:
                moveStrategy = new PathFindingMoveStrategy();
                symbol = Constants.PathFindingGhost;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null);
        }

        return new Ghost(coordinate, symbol, moveStrategy);
    }
}
