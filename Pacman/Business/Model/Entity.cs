namespace Pacman.Business.Model;

public abstract class Entity
{
    public Coordinate Coordinate { get; protected set; }
    public char Symbol { get; protected set; }

    protected Entity(Coordinate coordinate, char symbol)
    {
        Coordinate = coordinate;
        Symbol = symbol;
    }
}
