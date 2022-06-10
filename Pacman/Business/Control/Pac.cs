using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control;

public record Pac(Coordinate Coordinate, char Symbol, IMoveStrategy MoveStrategy) :
    MovableEntity(Coordinate, Symbol, MoveStrategy)
{
    public override GameState PlayTurn(GameState gameState)
    {
        var newCoord =  MoveStrategy.GetMove(this, gameState.Walls, gameState);

        return gameState with
        {
            Pac = gameState.Pac with
            {
                Coordinate = newCoord,
                Symbol = _getSymbol(Coordinate, newCoord)
            }
        };
    }

    private char _getSymbol(Coordinate originalCoord, Coordinate newCoord)
    {
        var (x1, y1) = originalCoord;
        var (x2, y2) = newCoord;
        var coordDisplacement = (x2 - x1, y2 - y1);

        return coordDisplacement switch
        {
            (_, > 0) => Constants.PacDown,
            (_, < 0) => Constants.PacUp,
            (< 0, _) => Constants.PacLeft,
            (> 0, _) => Constants.PacRight,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
