using Pacman.Business.Model;

namespace Pacman.Business.Control.MoveStrategies;

public interface IMoveStrategy
{
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState);
}