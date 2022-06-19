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

    public GameEngine(IEnumerable<IEntity> entities, GhostFactory ghostFactory, ISelector<Coordinate> selector)
    {
        _ghostFactory = ghostFactory;
        _selector = selector;
        InitialiseEntities(entities);
    }

    private void InitialiseEntities(IEnumerable<IEntity> entities)
    {
        int length = 0, width = 0;
        
        foreach (var entity in entities)
        {
            length = Math.Max(length, entity.Coordinate.Y + 1);
            width = Math.Max(width, entity.Coordinate.X + 1);
            
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
        }

        GameState = new GameState(new Size(width, length), Constants.PacStartingLives, Constants.StartRound, 
            0, GameStatus.Running, _pac, _ghosts, _walls, _pellets.Values);
    }
    
    public void PlayRound(Direction direction)
    {
        _pac.SetInput(direction);

        foreach (var movableEntity in _ghosts.Cast<IMovable>().Prepend(_pac)) 
            movableEntity.Move(GameState);

        UpdatePowerUp();
        UpdatePellets();

        if (_ghosts.Any(g => g.Coordinate == _pac.Coordinate))
            GameState = GameState with {GameStatus = GameStatus.Collided};
        else if (!GameState.GetPellets().Any())
            GameState = GameState with {GameStatus = GameStatus.RoundComplete};
    }

    private void UpdatePowerUp()
    {
        _pellets.TryGetValue(_pac.Coordinate, out var pellet);

        var powerUpRemaining = pellet is {Eaten: false, Symbol: Constants.MagicPellet} && !IsPacOnGhost() ? 
            Constants.PowerUpTurns : Math.Max(0, GameState.PowerUpRemaining - 1);

        GameState = GameState with {PowerUpRemaining = powerUpRemaining};
    }

    private void UpdatePellets()
    {
        if (_pellets.ContainsKey(_pac.Coordinate) && !IsPacOnGhost())
            _pellets[_pac.Coordinate].Eaten = true;
    }

    private bool IsPacOnGhost() => _ghosts.Any(g => g.Coordinate == _pac.Coordinate);
    
    public void ResetRound()
    {
        foreach (var resetable in _ghosts.Cast<IResetable>().Append(_pac))
            resetable.ResetState();
        
        var newLives = GameState.Lives - 1;
        GameState = GameState with
        {
            Lives = newLives,
            GameStatus = newLives == 0 ? GameStatus.GameComplete : GameStatus.Running
        };
    }
    
    public void IncreaseRound()
    {
        foreach (var resetable in _ghosts.Cast<IResetable>().Concat(_pellets.Values).Append(_pac))
            resetable.ResetState();
        
        AddGhost();
        GameState = GameState with
        {
            Round = GameState.Round + 1,
            GameStatus = GameState.Round > Constants.MaxRounds ? GameStatus.GameComplete : GameStatus.Running
        };
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
