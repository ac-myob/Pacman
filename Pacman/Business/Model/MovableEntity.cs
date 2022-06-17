namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity {
    
    private readonly Coordinate _startingCoordinate;

    protected MovableEntity(Coordinate coordinate, char symbol) : base(coordinate, symbol)
    {
        _startingCoordinate = coordinate;
    }

    public void ResetCoordinate() => Coordinate = _startingCoordinate;
}
