using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Variables;

namespace Pacman.Business.Model;

public record GameState(Size Size,
    int Lives,
    int Round,
    Pac Pac,
    IEnumerable<Ghost> Ghosts,
    IEnumerable<Wall> Walls,
    IEnumerable<Pellet> Pellets)
{
    public int Lives { get; private set; } = Lives;
    public int PowerUpRemaining { get; private set; }
    public int Round { get; private set; } = Round;

    public void DecreaseLife() => Lives = Math.Max(0, Lives - 1);

    public void IncrementRound() => ++Round;

    public void DecreasePowerUp() => PowerUpRemaining = Math.Max(0, PowerUpRemaining - 1);

    public void ResetPowerUp() => PowerUpRemaining = Constants.PowerUpTurns;
}
