using Pacman.Variables;

namespace Pacman.Business.Model;

public class Wall : Entity
{
    public Wall(Coordinate coordinate) : base(coordinate, Constants.Wall) { }
}
