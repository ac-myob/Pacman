using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class Pac : IMovable, IResetable
{
    private readonly Coordinate _startCoord;
    private readonly char _startSymbol;
    private Direction _chosenDirection;

    public Coordinate Coordinate { get; private set; }
    public char Symbol { get; private set; }

    public Pac(Coordinate coordinate, char symbol)
    {
        _startCoord = coordinate;
        Coordinate = coordinate;
        _startSymbol = symbol;
        Symbol = symbol;
    }

    public void SetInput(Direction direction) => _chosenDirection = direction;

    public void Move(GameState gameState)
    {
        var newCoord = Coordinate.Shift(_chosenDirection, gameState.Size);

        if (gameState.Walls.ContainsKey(newCoord)) return;

        Coordinate = newCoord;
        Symbol = GetSymbol(_chosenDirection);
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

    public void ResetState()
    {
        Coordinate = _startCoord;
        Symbol = _startSymbol;
    }
}
