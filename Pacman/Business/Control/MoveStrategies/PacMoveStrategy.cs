using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;

namespace Pacman.Business.Control.MoveStrategies;

public class PacMoveStrategy : IMoveStrategy
{
    private readonly IWriter _writer;
    private readonly Query _query;

    public PacMoveStrategy(IReader reader, IWriter writer)
    {
        _writer = writer;
        _query = new Query(reader, writer);
    }
    
    public Coordinate GetMove(MovableEntity movableEntity, IEnumerable<Entity> obstacles, GameState gameState)
    {
        var obstaclesArr = obstacles.ToArray();
        var coordinate = movableEntity.Coordinate;
        bool blockedByObstacle;
        Coordinate newCoord;
        
        do
        {
            var keyPress = _query.GetKeyPress(Constants.ValidKeysRegex, Messages.InvalidKeyPress);
            var chosenDirection = _getDirection(keyPress);
            newCoord = gameState.GetNewCoord(coordinate, chosenDirection, obstaclesArr);
            blockedByObstacle = newCoord == coordinate;

            if (blockedByObstacle) _writer.Write(Messages.WallObstruction);

        } while (blockedByObstacle);

        return newCoord;
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
}