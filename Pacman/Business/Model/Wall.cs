using Pacman.Variables;

namespace Pacman.Business.Model;

public class Wall : IEntity 
{
    public Coordinate Coordinate { get; }
    public char Symbol { get; }

    public Wall(Coordinate coordinate, char symbol = Constants.Wall)
    {
        Coordinate = coordinate;
        Symbol = symbol;
    }
}
