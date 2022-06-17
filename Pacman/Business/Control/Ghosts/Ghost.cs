using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;

namespace Pacman.Business.Control.Ghosts;

public class Ghost : MovableEntity
{
    private readonly IMoveStrategy _moveStrategy;

    public Ghost(Coordinate coordinate, char symbol, IMoveStrategy moveStrategy) : base(coordinate, symbol)
    {
        _moveStrategy = moveStrategy;
    }
    
    public void PlayTurn(GameState gameState)
    {
        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
        var currentStrategy = gameState.PowerUpRemaining > 0 ? new FleeMoveStrategy() : _moveStrategy;
        var newCoord = currentStrategy.GetMove(Coordinate, obstacles, gameState);

        Coordinate = newCoord;
    }
}
