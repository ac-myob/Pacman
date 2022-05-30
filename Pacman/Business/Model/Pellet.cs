using Pacman.Variables;

namespace Pacman.Business.Model;

public class Pellet : Entity
{
    public Pellet(Coordinate coordinate) : base(coordinate, Constants.Pellet)
    {
    }
}