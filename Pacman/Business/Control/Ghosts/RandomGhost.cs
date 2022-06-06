using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public record RandomGhost(Coordinate Coordinate, int Id, ISelector<Coordinate> Selector) :
    MovableEntity(Coordinate, Constants.RandomGhost, Id)
{
    private ISelector<Coordinate> Selector { get; } = Selector;
    
    public override GameState Move(GameState gameState)
    {
        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
            select gameState.GetNewCoord(Coordinate, direction, obstacles) into currentCoord 
            where currentCoord != Coordinate select currentCoord).ToArray();

        if (!posCoords.Any()) return gameState;

        var newCoord = Selector.SelectFrom(posCoords);
        
        return gameState.UpdateGhostCoordinate(Id, newCoord);
    }
}
