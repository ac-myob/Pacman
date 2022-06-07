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

namespace Pacman.Tests.GhostTests;

public class GreedyGhostTests
{
    private readonly GameState _gameState = new(
        It.IsAny<Size>(),
        Constants.PacStartingLives,
        It.IsAny<int>(),
        It.IsAny<Pac>(),
        It.IsAny<IEnumerable<BaseGhost>>(),
        Array.Empty<Wall>(),
        Array.Empty<Pellet>()
    );
    private readonly Pac _pac;

    public GreedyGhostTests()
    {
        _pac = new Pac(
            new Coordinate(),
            Constants.PacStart,
            It.IsAny<int>(),
            It.IsAny<IReader>(),
            It.IsAny<IWriter>());
    }
    
    [Theory]
    [MemberData(nameof(NoObstaclesTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenNoObstacles(
        Size mapSize, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = _gameState with
        {
            Size = mapSize,
            Pac = _pac with {Coordinate = pacCoord},
            Ghosts = new BaseGhost[] {new GreedyGhost(ghostCoord, It.IsAny<int>())}
        };
        
        var actualGameState = gameState.Ghosts.Single().Move(gameState);
        
        Assert.Equal(expectedCoord, actualGameState.Ghosts.Single().Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenWalls(
        Size mapSize, IEnumerable<Wall> walls, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = _gameState with
        {
            Size = mapSize,
            Pac = _pac with {Coordinate = pacCoord},
            Ghosts = new BaseGhost[] {new GreedyGhost(ghostCoord, It.IsAny<int>())},
            Walls = walls
        };
        
        var actualGameState = gameState.Ghosts.Single().Move(gameState);
        
        Assert.Equal(expectedCoord, actualGameState.Ghosts.Single().Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void Move_MovesGhostToMinimallyDistantPositionFromPac_GivenOtherGhosts(
        Size mapSize, IList<BaseGhost> ghosts, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = _gameState with
        {
            Size = mapSize,
            Pac = _pac with {Coordinate = pacCoord},
            Ghosts = new BaseGhost[] {new GreedyGhost(ghostCoord, It.IsAny<int>())}.Concat(ghosts)
        };
        
        var actualGameState = gameState.Ghosts.First().Move(gameState);
        
        Assert.Equal(expectedCoord, actualGameState.Ghosts.First().Coordinate);
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
            new BaseGhost[] {new RandomGhost(new Coordinate(2, 1), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 1), 
            new Coordinate(1, 1)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new BaseGhost[] {new RandomGhost(new Coordinate(1, 2), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(2, 1)
        };
    }
}
