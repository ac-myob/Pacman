using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public static class GameStateModifier
{
    public static GameState UpdatePellets(this GameState gameState)
    {
        if (gameState.IsPacOnGhost()) return gameState;
        
        return gameState with
        {
            Pellets = gameState.Pellets.Where(p => p.Coordinate != gameState.Pac.Coordinate),
        };
    }

    public static GameState UpdatePowerUp(this GameState gameState)
    {
        var onMagicPellet = gameState.Pellets.Any(p =>
            p.Coordinate == gameState.Pac.Coordinate && p.Symbol == Constants.MagicPellet);
        
        if (onMagicPellet) return gameState with {PowerUpRemaining = Constants.PowerUpTurns};

        return gameState with {PowerUpRemaining = Math.Max(gameState.PowerUpRemaining - 1, 0)};
    }
}
