using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class RandomGhost : MovableEntity
{
    private readonly ISelector<Coordinate> _selector;

    public RandomGhost(Coordinate coordinate, ISelector<Coordinate> selector) : base(coordinate)
    {
        _selector = selector;
    }

    public override void Move(Size size, IEnumerable<Entity> obstacles)
    {
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction))
            select GetNewCoord(direction, size, obstacles) into currentCoord 
            where currentCoord != Coordinate select currentCoord).ToArray();

        Coordinate = _selector.Select(posCoords);
    }
}