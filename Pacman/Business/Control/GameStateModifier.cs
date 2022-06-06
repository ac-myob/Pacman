using Pacman.Business.Model;

namespace Pacman.Business.Control;

public static class GameStateModifier
{
    public static GameState UpdatePellets(this GameState gameState)
    {
        if (gameState.IsPacOnGhost()) return gameState;
        
        return gameState with
        {
            Pellets = gameState.Pellets.Where(p => p.Coordinate != gameState.Pac.Coordinate)
        };
    }
    
    public static GameState UpdateGhostCoordinate(this GameState gameState, int id, Coordinate newCoord)
    {
        return gameState with
        {
            Ghosts = gameState.Ghosts.Select(g => g.Id != id ? g : g with{Coordinate = newCoord})
        };
    }
}