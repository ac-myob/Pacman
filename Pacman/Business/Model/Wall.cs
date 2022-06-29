namespace Pacman.Business.Model;

public class Wall : IEntity 
{
    public Coordinate Coordinate { get; }
    public char Symbol { get; }

    public Wall(Coordinate coordinate, char symbol)
    {
        Coordinate = coordinate;
        Symbol = symbol;
    }
}
