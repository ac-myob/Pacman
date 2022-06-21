using System.Text;
using Pacman.Business.Control;
using Pacman.Business.Model;

namespace Pacman.Variables;

public static class Messages
{
    public const string InvalidKeyPress = "Invalid key press. Use arrow keys to move:\n";
    public const string WallObstruction = "Pacman cannot move into a wall. Please try again:\n";
    public const string GhostCollision = "\nYou collided with a ghost! Press any key to continue...";
    public const string RoundComplete = "\nRound complete! Press any key to continue...";

    public static string GetTurnInfo(GameState gameState)
    {
        var res = new StringBuilder();
        res.Append($"Lives: {new string(Constants.Heart, gameState.Pac.Lives)}\n");
        res.Append($"Round: {Math.Min(gameState.Round, Constants.MaxRounds)}/{Constants.MaxRounds}\n");
        res.Append($"Pellets remaining: {gameState.GetPellets().Count()}\n");

        if (gameState.Pac.PowerUp > 0)
            res.Append($"Power up timer: {gameState.Pac.PowerUp}\n");

        return res.ToString();
    }
}
