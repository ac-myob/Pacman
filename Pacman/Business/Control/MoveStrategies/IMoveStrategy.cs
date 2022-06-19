using Pacman.Business.Model;

namespace Pacman.Business.Control.MoveStrategies;

public interface IMoveStrategy
{
    public Coordinate GetMove(Coordinate startingCoord, Func<Coordinate, bool> isBlocked, GameState gameState);
}
