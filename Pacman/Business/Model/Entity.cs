namespace Pacman.Business.Model;

public abstract class Entity
{
    public Coordinate Coordinate { get; protected set; }

    protected Entity(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }
}
