using Pacman.Business.Control.Ghosts;
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
    
    public Coordinate GetMove(Coordinate startingCoord, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
                select gameState.GetNewCoord(startingCoord, direction, obstaclesArr) into currentCoord 
                where currentCoord != startingCoord select currentCoord).ToArray();

        return !posCoords.Any() ? startingCoord : _selector.SelectFrom(posCoords);
    }
}
