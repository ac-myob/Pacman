using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class RandomMoveStrategy : IMoveStrategy
{
    private readonly ISelector<Coordinate> _selector;

    public RandomMoveStrategy(ISelector<Coordinate> selector)
    {
        _selector = selector;
    }
    
    public Coordinate GetMove(Coordinate startingCoord, Func<Coordinate, bool> isBlocked, GameState gameState)
    {
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
            select startingCoord.Shift(direction, gameState.Size) into currentCoord 
            where currentCoord != startingCoord && !isBlocked(currentCoord) select currentCoord).ToArray();

        return !posCoords.Any() ? startingCoord : _selector.SelectFrom(posCoords);
    }
}
