namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity
{
    protected MovableEntity(Coordinate coordinate) : base(coordinate) { }

    public abstract void Move(Size size, IEnumerable<Entity> obstacles);
}
