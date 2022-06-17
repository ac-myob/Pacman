using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class Pac : MovableEntity
{
    public Pac(Coordinate coordinate, char symbol) : base(coordinate, symbol) { }
    
    public void PlayTurn(GameState gameState, Direction direction)
    {
        Coordinate = gameState.GameStateExtensions(Coordinate, direction, gameState.Walls);
        Symbol = GetSymbol(direction);
    }

    private static char GetSymbol(Direction direction)
    {
        return direction switch
        {
            Direction.South => Constants.PacDown,
            Direction.North => Constants.PacUp,
            Direction.West => Constants.PacLeft,
            Direction.East => Constants.PacRight,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public void ResetSymbol() => Symbol = Constants.PacStart;
}
