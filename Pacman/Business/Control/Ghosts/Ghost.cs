using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class Ghost : IMovable, IResetable
{
    private readonly IMoveStrategy _moveStrategy;
    private readonly IMoveStrategy _fleeStrategy = new FleeMoveStrategy();
    private readonly Coordinate _startCoord;
    private readonly char _startSymbol;
    public Coordinate Coordinate { get; private set; }
    public char Symbol { get; private set; }
    
    public void Move(GameState gameState)
    {
        var isFeared = gameState.PowerUpRemaining > 0;
        var currentStrategy = isFeared ? _fleeStrategy : _moveStrategy;
        bool IsBlocked(Coordinate coordinate) => 
            gameState.Walls.ContainsKey(coordinate) || gameState.Ghosts.Any(g => g.Coordinate == coordinate);
        
        Coordinate = currentStrategy.GetMove(Coordinate, IsBlocked, gameState);
        Symbol = isFeared ? Constants.FleeGhost : _startSymbol;
    }

    public Ghost(Coordinate coordinate, char symbol, IMoveStrategy moveStrategy)
    {
        _moveStrategy = moveStrategy;
        Coordinate = coordinate;
        _startCoord = coordinate;
        Symbol = symbol;
        _startSymbol = symbol;
    }
    
    public void ResetState() => Coordinate = _startCoord;
}
