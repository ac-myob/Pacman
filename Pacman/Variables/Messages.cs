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
        res.AppendLine($"Lives: {new string(Constants.Heart, gameState.Pac.Lives)}");
        res.AppendLine($"Round: {Math.Min(gameState.Round, Constants.MaxRounds)}/{Constants.MaxRounds}");
        res.AppendLine($"Pellets remaining: {gameState.GetPellets().Count()}");

        if (gameState.Pac.PowerUp > 0)
            res.AppendLine($"Power up timer: {gameState.Pac.PowerUp}");

        return res.ToString();
    }
}
