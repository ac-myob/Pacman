using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Variables;

namespace Pacman.Business.Model;

public class GameState
{
    private IDictionary<Coordinate, Pellet> _pellets;
    private readonly IList<Pellet> _removedPellets = new List<Pellet>();
    public Size Size { get; }
    public int Round { get; private set; } = Constants.StartRound;
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

    public void RemovePellet(Coordinate coordinate)
    {
        if (_pellets.ContainsKey(coordinate))
            _removedPellets.Add(_pellets[coordinate]);
        
        _pellets.Remove(coordinate);
    }

    public void ResetPellets() => _pellets = _removedPellets.ToDictionary(p => p.Coordinate, p => p);

    public void AddGhost(ISelector<Coordinate> selector)
    {
        var ghostTypes = Enum.GetValues(typeof(GhostType)).Cast<GhostType>();
        var chosenGhostType = ghostTypes.ElementAt(Round - 1);
        var coordinate = selector.Select(Pellets.Select(p => p.Coordinate));
        
        MovableEntity newGhost = chosenGhostType switch
        {
            GhostType.Random => new RandomGhost(coordinate, selector),
            GhostType.Greedy => new GreedyGhost(coordinate, Pac),
            GhostType.PathFinding => new PathFindingGhost(coordinate, Pac),
            _ => throw new ArgumentOutOfRangeException()
        };

        Ghosts = Ghosts.Append(newGhost);
    }

    public void ResetMovableEntities()
    {
        foreach (var movableEntity in MovableEntities)
            movableEntity.ResetCoordinate();
    }
    
    public bool IsPacOnGhost() => Ghosts.Any(g => g.Coordinate == Pac.Coordinate);

    public void IncreaseRound() => Round = Math.Min(Round + 1, Constants.MaxRounds);

    public bool FinishedGame() => Round == Constants.MaxRounds;
}
