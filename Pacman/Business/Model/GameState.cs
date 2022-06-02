using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Variables;

namespace Pacman.Business.Model;

public class GameState
{
    private readonly IDictionary<Coordinate, Pellet> _pellets;
    public Size Size { get; }
    public Pac Pac { get; }
    public IEnumerable<MovableEntity> MovableEntities => new MovableEntity[] {Pac}.Concat(Ghosts);
    public IEnumerable<Entity> Walls { get; }
    public IEnumerable<Entity> Pellets => _pellets.Values;
    public IEnumerable<MovableEntity> Ghosts { get; private set; }
    
    public GameState(Size size, Pac pac, IEnumerable<Wall> walls, IEnumerable<Pellet> pellets, IEnumerable<MovableEntity> ghosts)
    {
        Size = size;
        Pac = pac;
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
            GhostType.Greedy => new GreedyGhost(coordinate, Pac),
            GhostType.PathFinding => new PathFindingGhost(coordinate, Pac),
            _ => throw new ArgumentOutOfRangeException(nameof(ghostType), ghostType, null)
        };

        Ghosts = Ghosts.Append(newGhost);
    }
}
