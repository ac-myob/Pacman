using Pacman.Business.Control;
using Pacman.Variables;

namespace Pacman.Business.Model;

public abstract class MovableEntity : Entity
{
    protected MovableEntity(Coordinate coordinate) : base(coordinate) { }

    public abstract void Move(GameState gameState);
}
