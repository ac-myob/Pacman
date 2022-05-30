using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Business.View;
using Xunit;

namespace Pacman.Tests;

public class GreedyGhostTests
{
    [Theory]
    [MemberData(nameof(NoObstaclesTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenNoObstacles(
        Size mapSize, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var pac = new Pac(pacCoord, It.IsAny<IReader>(), It.IsAny<IWriter>());
        var greedyGhost = new GreedyGhost(ghostCoord, pac);
        var gameState = new GameState(
            mapSize, 
            pac, 
            Array.Empty<Wall>(), 
            It.IsAny<Pellet[]>(), 
            Array.Empty<MovableEntity>()
            );

        greedyGhost.Move(gameState);
        
        Assert.Equal(expectedCoord, greedyGhost.Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenWalls(
        Size mapSize, IEnumerable<Wall> walls, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var pac = new Pac(pacCoord, It.IsAny<IReader>(), It.IsAny<IWriter>());
        var greedyGhost = new GreedyGhost(ghostCoord, pac);
        var gameState = new GameState(
            mapSize, 
            pac, 
            walls, 
            It.IsAny<Pellet[]>(),
            Array.Empty<MovableEntity>()
            );

        greedyGhost.Move(gameState);
        
        Assert.Equal(expectedCoord, greedyGhost.Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenOtherGhosts(
        Size mapSize, IEnumerable<MovableEntity> ghosts, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var pac = new Pac(pacCoord, It.IsAny<IReader>(), It.IsAny<IWriter>());
        var greedyGhost = new GreedyGhost(ghostCoord, pac);
        var gameState = new GameState(
            mapSize, 
            pac, 
            Array.Empty<Wall>(), 
            It.IsAny<Pellet[]>(),
            ghosts
            );

        greedyGhost.Move(gameState);
        
        Assert.Equal(expectedCoord, greedyGhost.Coordinate);
    }

    private static IEnumerable<object[]> NoObstaclesTestData()
    {
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(0, 0), 
            new Coordinate(2, 2), 
            new Coordinate(0, 2)
        };
        
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1), 
            new Coordinate(2, 2), 
            new Coordinate(1, 2)
        };
        
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1), 
            new Coordinate(1, 1), 
            new Coordinate(1, 1)
        };
    }
    
    private static IEnumerable<object[]> WallsTestData()
    {
        yield return new object[]
        {
            new Size(4, 4),
            new Wall[] {new(new Coordinate(2, 1))},
            new Coordinate(1, 1), 
            new Coordinate(3, 1), 
            new Coordinate(1, 1)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new Wall[] {new(new Coordinate(1, 2))},
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(2, 1)
        };
    }
    
    private static IEnumerable<object[]> GhostsTestData()
    {
        yield return new object[]
        {
            new Size(4, 4),
            new MovableEntity[] {new RandomGhost(new Coordinate(2, 1), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 1), 
            new Coordinate(1, 1)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new MovableEntity[] {new RandomGhost(new Coordinate(1, 2), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(2, 1)
        };
    }
}
