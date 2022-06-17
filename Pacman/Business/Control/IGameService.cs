using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public interface IGameService
{
    public void Tick(Direction inputDirection);
    public GameState GameState { get; }
}
