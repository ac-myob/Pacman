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
    
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var coordinate = movableEntity.Coordinate;
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
                select gameState.GetNewCoord(coordinate, direction, obstaclesArr) into currentCoord 
                where currentCoord != coordinate select currentCoord).ToArray();

        return !posCoords.Any() ? coordinate : _selector.SelectFrom(posCoords);
    }
}