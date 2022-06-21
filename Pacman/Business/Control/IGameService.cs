using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public interface IGameService
{
    public void PlayRound(Direction inputDirection);
    public void ResetRound();
    public void IncreaseRound();
    public bool IsGameFinished();
    public bool IsRoundComplete();
    public bool IsGameRunning();
    public bool IsPacOnGhost();
    public bool IsDirectionValid(Direction inputDirection);
    public string GameMap();
}
