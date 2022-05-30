using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Model;
using Pacman.Business.View;
using Xunit;

namespace Pacman.Tests;

public class PathFindingGhostTests
{
    [Theory]
    [MemberData(nameof(NoObstaclesTestData))]
    public void Move_MovesGhostTowardPac_GivenNoObstacles(
        Size mapSize, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var pac = new Pac(pacCoord, It.IsAny<IReader>(), It.IsAny<IWriter>());
        var pathFindingGhost = new PathFindingGhost(ghostCoord, pac);
        var gameState = new GameState(
            mapSize,
            pac,
            Array.Empty<Wall>(),
            Array.Empty<MovableEntity>()
            );

        pathFindingGhost.Move(gameState);
        
        Assert.Equal(expectedCoord, pathFindingGhost.Coordinate);
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
}