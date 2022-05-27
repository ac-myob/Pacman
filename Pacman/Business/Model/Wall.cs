namespace Pacman.Business.Model;

public class Wall
{
    public Coordinate Coordinate { get; }
    
    public Wall(Coordinate coordinate)
    {
        Coordinate = coordinate;
    }
}
