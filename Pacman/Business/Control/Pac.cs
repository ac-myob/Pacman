using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;

namespace Pacman.Business.Control;

public class Pac : MovableEntity
{
    private readonly Query _query;
    private readonly IWriter _writer;

    public Pac(Coordinate coordinate, IReader reader, IWriter writer) : base(coordinate, Constants.PacStart)
    {
        _query = new Query(reader, writer);
        _writer = writer;
    }
    
    public override void Move(GameState gameState)
    {
        string keyPress;
        bool blockedByWall;
        Coordinate newCoord;
        
        do
        {
            keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
            var chosenDirection = _getDirection(keyPress);
            newCoord = gameState.GetNewCoord(Coordinate, chosenDirection, gameState.Walls);
            blockedByWall = newCoord == Coordinate;

            if (blockedByWall) _writer.Write(Messages.WallObstruction);

        } while (blockedByWall);

        gameState.RemovePellet(newCoord);
        _updateSymbol(keyPress);
        Coordinate = newCoord;
    }

    private Direction _getDirection(string keyPress)
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

    private void _updateSymbol(string keyPress)
    {
        Symbol = keyPress switch
        {
            Constants.UpKey => Constants.PacUp,
            Constants.DownKey => Constants.PacDown,
            Constants.LeftKey => Constants.PacLeft,
            Constants.RightKey => Constants.PacRight,
            _ => throw new ArgumentOutOfRangeException(keyPress)
        };
    }
}
