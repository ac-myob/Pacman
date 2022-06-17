using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;

namespace Pacman.Business.Control.MoveStrategies;

public interface IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, IEnumerable<Entity> obstacles, GameState gameState);
}
