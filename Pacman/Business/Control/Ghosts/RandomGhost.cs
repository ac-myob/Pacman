using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public class RandomGhost : MovableEntity
{
    private readonly ISelector<Coordinate> _selector;

    public RandomGhost(Coordinate coordinate, ISelector<Coordinate> selector) : base(coordinate, Constants.RandomGhost)
    {
        _selector = selector;
    }

    public override void Move(GameState gameState)
    {
        var obstacles = gameState.Walls.Concat(gameState.Ghosts).ToArray();
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
            select gameState.GetNewCoord(Coordinate, direction, obstacles) into currentCoord 
            where currentCoord != Coordinate select currentCoord).ToArray();

        Coordinate = _selector.Select(posCoords);
    }
}
