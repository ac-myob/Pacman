using Pacman.Business.Control;
using Pacman.Variables;

namespace Pacman.Business.Model;

public class Pac : IMovable, IResetable
{
    private readonly Coordinate _startCoord;
    private readonly char _startSymbol;
    private Direction _chosenDirection;
    private bool _isEating;

    public Coordinate Coordinate { get; private set; }
    public char Symbol { get; private set; }
    public int Lives { get; private set; }
    public int PowerUp { get; private set; }

    public Pac(Coordinate coordinate, char symbol, int lives)
    {
        _startCoord = coordinate;
        Coordinate = coordinate;
        _startSymbol = symbol;
        Symbol = symbol;
        Lives = lives;
    }

    public void SetInput(Direction direction) => _chosenDirection = direction;

    public void Move(GameState gameState)
    {
        var newCoord = Coordinate.Shift(_chosenDirection, gameState.Size);

        if (gameState.Walls.ContainsKey(newCoord)) return;

        Coordinate = newCoord;
        Symbol = GetSymbol(_chosenDirection);
        _isEating = !_isEating;
    }

    private char GetSymbol(Direction direction)
    {
        return direction switch
        {
            Direction.South => _isEating ? Constants.PacVert : Constants.PacDown,
            Direction.North => _isEating ? Constants.PacVert : Constants.PacUp,
            Direction.West => _isEating ? Constants.PacHorz : Constants.PacLeft,
            Direction.East => _isEating ? Constants.PacHorz : Constants.PacRight,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public void ResetState()
    {
        Coordinate = _startCoord;
        Symbol = _startSymbol;
    }

    public void ReduceLife() => Lives = Math.Max(0, Lives - 1);

    public void AddPowerUp() => PowerUp = Constants.PowerUpTurns;

    public void ReducePowerUp() => PowerUp = Math.Max(0, PowerUp - 1);
}
