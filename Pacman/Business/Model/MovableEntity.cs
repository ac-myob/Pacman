namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity
{
    private readonly Coordinate _startCoord;

    protected MovableEntity(Coordinate coordinate, char symbol) : base(coordinate, symbol)
    {
        _startCoord = coordinate;
    }

    public abstract void Move(GameState gameState);

    public void ResetCoordinate() => Coordinate = _startCoord;
}
