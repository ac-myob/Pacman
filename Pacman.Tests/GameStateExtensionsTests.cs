using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class GameStateExtensionsTests
{
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesDoNotShareSameCoordinate()
    {
        var pac = new Pac(new Coordinate(0, 0), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            new Wall[] { new(new Coordinate(1, 1))},
            It.IsAny<Pellet[]>(),
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(0, 2), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(2, 0), pac),
                new PathFindingGhost(new Coordinate(2, 2), pac)
            }
        );
        var expectedString = $"{Constants.PacStart}{Constants.Blank}{Constants.GreedyGhost}\n" +
                             $"{Constants.Blank}{Constants.Wall}{Constants.Blank}\n" +
                             $"{Constants.RandomGhost}{Constants.Blank}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
    
    [Fact]
    public void GetString_ReturnsStringRepresentationOfGame_WhenEntitiesShareSameCoordinate()
    {
        var pac = new Pac(new Coordinate(0, 0), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            new Wall[] { new(new Coordinate(1, 1))},
            It.IsAny<Pellet[]>(),
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(0, 2), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(2, 0), pac),
                new PathFindingGhost(new Coordinate(2, 2), pac)
            }
        );
        var expectedString = $"{Constants.PacStart}{Constants.Blank}{Constants.GreedyGhost}\n" +
                             $"{Constants.Blank}{Constants.Wall}{Constants.Blank}\n" +
                             $"{Constants.RandomGhost}{Constants.Blank}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
}