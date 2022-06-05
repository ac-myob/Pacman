using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;

namespace Pacman.Business.Control.Ghosts;

public record RandomGhost(Coordinate Coordinate, ISelector<Coordinate> Selector, int Id) :
    MovableEntity(Coordinate, Constants.RandomGhost, Id)
{
    public override GameState Move(GameState gameState)
    {
        var obstacles = gameState.Walls.Cast<Entity>().Concat(gameState.Ghosts).ToArray();
        var posCoords = 
            (from Direction direction in Enum.GetValues(typeof(Direction)) 
            select gameState.GetNewCoord(Coordinate, direction, obstacles) into currentCoord 
            where currentCoord != Coordinate select currentCoord).ToArray();

        if (!posCoords.Any()) return gameState;

        var newCoord = Selector.Select(posCoords);
        
        return gameState with
        {
            Ghosts = gameState.Ghosts.Select(g => g.Id != Id ? g : new RandomGhost(newCoord, Selector, Id))
        };
    }
}
