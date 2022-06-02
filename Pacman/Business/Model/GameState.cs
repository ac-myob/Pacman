using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Variables;

namespace Pacman.Business.Model;

public class GameState
{
    private readonly Pac _pac;
    private readonly IDictionary<Coordinate, Pellet> _pellets;
    public Size Size { get; }
    public IEnumerable<MovableEntity> MovableEntities => new MovableEntity[] {_pac}.Concat(Ghosts);
    public IEnumerable<Entity> Walls { get; }
    public IEnumerable<Pellet> Pellets => _pellets.Values;
    public IEnumerable<MovableEntity> Ghosts { get; private set; }
    
    public GameState(Size size, Pac pac, IEnumerable<Wall> walls, IEnumerable<Pellet> pellets, IEnumerable<MovableEntity> ghosts)
    {
        Size = size;
        _pac = pac;
        Walls = walls;
        _pellets = pellets.ToDictionary(x => x.Coordinate, x => x);
        Ghosts = ghosts;
    }

    public void RemovePellet(Coordinate coordinate) => _pellets.Remove(coordinate);

    public void AddGhost(GhostType ghostType, Coordinate coordinate)
    {
        MovableEntity newGhost = ghostType switch
        {
            GhostType.Random => new RandomGhost(coordinate, new RandomSelector<Coordinate>()),
            GhostType.Greedy => new GreedyGhost(coordinate, _pac),
            GhostType.PathFinding => new PathFindingGhost(coordinate, _pac),
            _ => throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null)
        };

        Ghosts = Ghosts.Append(newGhost);
    }
}
