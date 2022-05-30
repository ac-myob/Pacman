using System;
using System.Collections.Generic;
using System.Linq;
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
            Array.Empty<Pellet>(),
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
        var pellets = new List<Pellet>();
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                pellets.Add(new Pellet(new Coordinate(i, j)));
        
        var pac = new Pac(new Coordinate(0, 0), It.IsAny<IReader>(), It.IsAny<IWriter>());
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            new Wall[] { new(new Coordinate(1, 1))},
            pellets,
            new MovableEntity[]
            {
                new RandomGhost(new Coordinate(0, 2), It.IsAny<ISelector<Coordinate>>()),
                new GreedyGhost(new Coordinate(2, 0), pac),
                new PathFindingGhost(new Coordinate(2, 2), pac)
            }
        );
        var expectedString = $"{Constants.PacStart}{Constants.Pellet}{Constants.GreedyGhost}\n" +
                             $"{Constants.Pellet}{Constants.Wall}{Constants.Pellet}\n" +
                             $"{Constants.RandomGhost}{Constants.Pellet}{Constants.PathFindingGhost}\n";
        
        Assert.Equal(expectedString, gameState.GetString());
    }
}