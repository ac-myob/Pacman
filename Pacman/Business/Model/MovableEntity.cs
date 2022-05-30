namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity
{
    protected MovableEntity(Coordinate coordinate, char symbol) : base(coordinate, symbol) { }

    public abstract void Move(GameState gameState);
}
