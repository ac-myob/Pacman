using System.Text;
using Pacman.Business.Model;

namespace Pacman.Variables;

public static class Messages
{
    public const string InvalidKeyPress = "Invalid key press. Use arrow keys to move:\n";
    public const string WallObstruction = "Pacman cannot move into a wall. Please try again:\n";
    public const string GhostCollision = "You collided with a ghost! Press any key to continue...";
    public const string RoundComplete = "Round complete! Press any key to continue...";

    public static string GetTurnInfo(GameState gameState)
    {
        var res = new StringBuilder();
        res.Append($"Lives: {new string(Constants.Heart, gameState.Lives)}\n");
        res.Append($"Round: {Math.Min(gameState.Round, Constants.MaxRounds)}/{Constants.MaxRounds}\n");
        res.Append($"Pellets remaining: {gameState.Pellets.Count()}\n");

        return res.ToString();
    }
    
    public static string GetGameOutcome(GameState gameState)
    {
        return gameState.Round > Constants.MaxRounds ? 
            "\nCongratulations! You win!\n" : 
            "\nYou lost! Better luck next time!\n";
    }
}
