using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;

namespace Pacman.Business.Control;

public record Pac(Coordinate Coordinate, char Symbol, int Id, int Lives, IReader Reader, IWriter Writer) : 
    MovableEntity(Coordinate, Symbol, Id)
{
    private IWriter Writer { get; } = Writer;
    private IReader Reader { get; } = Reader;

    public override GameState Move(GameState gameState)
    {
        var query = new Query(Reader, Writer);
        string keyPress;
        bool blockedByWall;
        Coordinate newCoord;
        
        do
        {
            keyPress = query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
            var chosenDirection = _getDirection(keyPress);
            newCoord = gameState.GetNewCoord(Coordinate, chosenDirection, gameState.Walls);
            blockedByWall = newCoord == Coordinate;

            if (blockedByWall) Writer.Write(Messages.WallObstruction);

        } while (blockedByWall);

        return gameState with
        {
            Pac = gameState.Pac with
            {
                Coordinate = newCoord,
                Symbol = _getSymbol(keyPress)
            }
        };
    }

    private static Direction _getDirection(string keyPress)
    {
        return keyPress switch
        {
            Constants.UpKey => Direction.North,
            Constants.DownKey => Direction.South,
            Constants.LeftKey => Direction.West,
            Constants.RightKey => Direction.East,
            _ => throw new ArgumentOutOfRangeException(keyPress)
        };
    }

    private static char _getSymbol(string keyPress)
    {
        return keyPress switch
        {
            Constants.UpKey => Constants.PacUp,
            Constants.DownKey => Constants.PacDown,
            Constants.LeftKey => Constants.PacLeft,
            Constants.RightKey => Constants.PacRight,
            _ => throw new ArgumentOutOfRangeException(keyPress)
        };
    }
}
