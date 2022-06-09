using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;

namespace Pacman.Business.Control.Ghosts;

public record Ghost(Coordinate Coordinate, char Symbol, int Id, IMoveStrategy MoveStrategy) :
    MovableEntity(Coordinate, Symbol, Id, MoveStrategy)
{
    public override GameState PlayTurn(GameState gameState)
    {
        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
        var currentStrategy = gameState.PowerUpRemaining > 0 ? new FleeMoveStrategy() : MoveStrategy;
        var newCoord = currentStrategy.GetMove(this, obstacles, gameState);
        var newGhosts = gameState.Ghosts.Select(g => g.Id != Id ? g : g with {Coordinate = newCoord});
        
        return gameState with {Ghosts = newGhosts};
    }
}