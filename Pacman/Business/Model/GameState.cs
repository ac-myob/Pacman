using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Variables;

namespace Pacman.Business.Model;

public class GameState
{
    public Size Size { get; }
    public int Lives { get; private set; }
    public int PowerUpRemaining { get; private set; }
    public int Round { get; private set; }
    public Pac Pac { get; }
    public IEnumerable<Ghost> Ghosts { get; }
    public IEnumerable<Wall> Walls { get; }
    public IEnumerable<Pellet> Pellets { get; }

    public GameState(
        Size size,
        int lives,
        int round,
        Pac pac,
        IEnumerable<Ghost> ghosts,
        IEnumerable<Wall> walls,
        IEnumerable<Pellet> pellets)
    {
        Size = size;
        Lives = lives;
        Round = round;
        Pac = pac;
        Ghosts = ghosts;
        Walls = walls;
        Pellets = pellets;
    }

    public void DecreaseLife() => Lives = Math.Max(0, Lives - 1);

    public void IncrementRound() => ++Round;

    public void DecreasePowerUp() => PowerUpRemaining = Math.Max(0, PowerUpRemaining - 1);

    public void ResetPowerUp() => PowerUpRemaining = Constants.PowerUpTurns;
}
