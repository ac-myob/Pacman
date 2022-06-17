using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Control.Sequence;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class GameEngine
{
    private Pac _pac = default!;
    private readonly List<Ghost> _ghosts = new();
    private readonly Dictionary<Coordinate, Pellet> _pellets = new();
    private readonly Dictionary<Coordinate, Wall> _walls = new();
    public GameState GameState { get; private set; } = default!;
    
    private readonly GhostFactory _ghostFactory;
    private readonly ISelector<Coordinate> _selector;
    private readonly GhostTypeSequence _ghostTypeSequence = new();

    public GameEngine((Size, IEnumerable<Entity>) worldInfo, GhostFactory ghostFactory, ISelector<Coordinate> selector)
    {
        _ghostFactory = ghostFactory;
        _selector = selector;
        InitialiseEntities(worldInfo);
    }

    private void InitialiseEntities((Size, IEnumerable<Entity>) worldInfo)
    {
        var (size, entities) = worldInfo;

        foreach (var entity in entities)
            switch (entity.Symbol)
            {
                case Constants.Wall:
                    _walls.Add(entity.Coordinate, (Wall) entity);
                    break;
                case Constants.Pellet:
                case Constants.MagicPellet:
                    _pellets.Add(entity.Coordinate, (Pellet) entity);
                    break;
                case Constants.RandomGhost:
                case Constants.GreedyGhost:
                case Constants.PathFindingGhost:
                    _ghosts.Add((Ghost) entity);
                    break;
                case Constants.PacStart:
                    _pac = (Pac) entity;
                    break;
            }

        GameState = new GameState(size, Constants.PacStartingLives, Constants.StartRound, 
            _pac, _ghosts, _walls.Values, _pellets.Values);
    }
    
    public void PlayRound(Direction direction)
    {
        _pac.PlayTurn(GameState, direction);

        foreach (var ghost in _ghosts) 
            ghost.PlayTurn(GameState);

        UpdatePowerUp();
        UpdatePellets();
    }

    private void UpdatePowerUp()
    {
        _pellets.TryGetValue(_pac.Coordinate, out var pellet);

        if (pellet is {Eaten: false, Symbol: Constants.MagicPellet})
            GameState.ResetPowerUp();
        else
            GameState.DecreasePowerUp();
    }

    private void UpdatePellets()
    {
        if (_pellets.ContainsKey(_pac.Coordinate) && _ghosts.All(g => g.Coordinate != _pac.Coordinate))
            _pellets[_pac.Coordinate].Eaten = true;
    } 
    
    public void ResetRound()
    {
        foreach (var movableEntity in GameState.GetMovableEntities())
            movableEntity.ResetCoordinate();
        
        GameState.Pac.ResetSymbol();
        GameState.DecreaseLife();
    }
    
    public void IncreaseRound()
    {
        foreach (var pellet in _pellets.Values)
            pellet.Eaten = false;
        
        foreach (var movableEntity in GameState.GetMovableEntities())
            movableEntity.ResetCoordinate();
        
        GameState.Pac.ResetSymbol();
        GameState.IncrementRound();
        AddGhost();
    }
    
    private void AddGhost()
    {
        var newGhostType = _ghostTypeSequence.GetNext();
        var posNewGhostCoords = _pellets.Keys.Except(_ghosts.Select(g => g.Coordinate)).ToArray();

        // If no space to add ghost, don't add ghost
        if (!posNewGhostCoords.Any()) return;
        
        var newGhostCoord = _selector.SelectFrom(posNewGhostCoords);
        var newGhost = _ghostFactory.GetGhost(newGhostType, newGhostCoord);
            
        _ghosts.Add(newGhost);
    }
}
