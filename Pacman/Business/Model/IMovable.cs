namespace Pacman.Business.Model;

public interface IMovable : IEntity
{
    void Move(GameState gameState);
}