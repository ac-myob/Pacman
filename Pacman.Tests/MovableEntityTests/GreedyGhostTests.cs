using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;
using Xunit;

namespace Pacman.Tests.MovableEntityTests;

public class GreedyGhostTests
{
    private readonly IMoveStrategy _moveStrategy = new GreedyMoveStrategy();
    
    [Theory]
    [MemberData(nameof(NoObstaclesTestData))]
    public void PlayTurn_MovesGhostToMinimallyDistantPositionFromPac_GivenNoObstacles(
        Size mapSize, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = mapSize,
            Pac = new Pac(pacCoord, It.IsAny<char>()),
            Ghosts = new[] {new Ghost(ghostCoord, It.IsAny<char>(), _moveStrategy)}
        };

        gameState.Ghosts.Single().PlayTurn(gameState);
        
        Assert.Equal(expectedCoord, gameState.Ghosts.Single().Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void PlayTurn_MovesGhostToMinimallyDistantPositionFromPac_GivenWalls(
        Size mapSize, IEnumerable<Wall> walls, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = mapSize,
            Pac = new Pac(pacCoord, It.IsAny<char>()),
            Ghosts = new[] {new Ghost(ghostCoord, It.IsAny<char>(), new GreedyMoveStrategy())},
            Walls = walls
        };
        
        gameState.Ghosts.Single().PlayTurn(gameState);
        
        Assert.Equal(expectedCoord, gameState.Ghosts.Single().Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void PlayTurn_MovesGhostToMinimallyDistantPositionFromPac_GivenOtherGhosts(
        Size mapSize, IList<Ghost> ghosts, Coordinate ghostCoord, Coordinate pacCoord, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = mapSize,
            Pac = new Pac(pacCoord, It.IsAny<char>()),
            Ghosts = new[] {new Ghost(ghostCoord, It.IsAny<char>(), new GreedyMoveStrategy())}.Concat(ghosts)
        };
        
        gameState.Ghosts.First().PlayTurn(gameState);
        
        Assert.Equal(expectedCoord, gameState.Ghosts.First().Coordinate);
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
            new[] {new Ghost(new Coordinate(2, 1), It.IsAny<char>(), It.IsAny<IMoveStrategy>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 1), 
            new Coordinate(1, 1)
        };
        
        yield return new object[]
        {
            new Size(4, 4),
            new[] {new Ghost(new Coordinate(1, 2), It.IsAny<char>(), It.IsAny<IMoveStrategy>())},
            new Coordinate(1, 1), 
            new Coordinate(3, 3), 
            new Coordinate(2, 1)
        };
    }
}
