using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests.GhostTests;

public class PathFindingGhostTests
{
    private readonly GameState _gameState = new(
        It.IsAny<Size>(),
        Constants.PacStartingLives,
        It.IsAny<int>(),
        It.IsAny<Pac>(),
        Array.Empty<BaseGhost>(),
        Array.Empty<Wall>(),
        Array.Empty<Pellet>()
    );
    
    [Theory]
    [MemberData(nameof(NoObstaclesTestData))]
    public void Move_MovesGhostTowardPac_GivenNoObstacles(
        Size mapSize, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = _gameState with
        {
            Pac = new Pac(pacCoord, Constants.PacStart, It.IsAny<int>(),
                It.IsAny<IReader>(), It.IsAny<IWriter>()),
            Size = mapSize,
            Ghosts = new BaseGhost[] {new PathFindingGhost(ghostCoord, It.IsAny<int>())}
        };

        var actualGameState = gameState.Ghosts.Single().Move(gameState);
        
        Assert.Equal(expectedCoord, actualGameState.Ghosts.Single().Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void Move_MovesGhostTowardPac_GivenWalls(
        Size mapSize, IEnumerable<Wall> walls, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = _gameState with
        {            
            Size = mapSize,
            Pac = new Pac(pacCoord, Constants.PacStart, It.IsAny<int>(), It.IsAny<IReader>(), 
                It.IsAny<IWriter>()),
            Ghosts = new BaseGhost[] {new PathFindingGhost(ghostCoord, It.IsAny<int>())},
            Walls = walls
        };

        var actualGameState = gameState.Ghosts.Single().Move(gameState);
        
        Assert.Equal(expectedCoord, actualGameState.Ghosts.Single().Coordinate);
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
            new Coordinate(0, 1)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new Wall[] {new(new Coordinate(1, 2))},
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(1, 0)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new Wall[]
            {
                new(new Coordinate(1, 0)), 
                new(new Coordinate(1, 2)), 
                new(new Coordinate(0, 1)),
                new(new Coordinate(2, 1))
            },
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(1, 1)
        };
    }
}
