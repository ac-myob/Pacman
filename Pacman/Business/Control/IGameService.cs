using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public interface IGameService
{
    public void PlayRound(Direction inputDirection);
    public void ResetRound();
    public void IncreaseRound();
    public GameState GameState { get; }
}
