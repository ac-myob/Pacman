using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Pacman.Variables;
using Xunit;
using Capture = Moq.Capture;

namespace Pacman.Tests.GhostTests;

public class RandomGhostTests
{
    private readonly Mock<ISelector<Coordinate>> _mockSelector = new();
    private readonly GameState _gameState = new(
        It.IsAny<Size>(),
        Constants.PacStartingLives,
        It.IsAny<int>(),
        It.IsAny<Pac>(),
        It.IsAny<IEnumerable<BaseGhost>>(),
        Array.Empty<Wall>(),
        Array.Empty<Pellet>()
    );

    [Fact]
    public void Move_MovesGhostToRandomValidPosition_GivenNoObstacles()
    {
        var gameState = _gameState with
        {
            Size = new Size(3, 3),
            Ghosts = new BaseGhost[] {new RandomGhost(new Coordinate(1, 1), It.IsAny<int>(), _mockSelector.Object)}
        };
        var expectedPosCoords = new[]
        {
            new Coordinate(1, 0), 
            new Coordinate(1, 2), 
            new Coordinate(2, 1), 
            new Coordinate(0, 1)
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);

        _mockSelector.Setup(_ => _.SelectFrom(Capture.With(match)));
        gameState.Ghosts.Single().Move(gameState);

        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void Move_MovesGhostToRandomValidPosition_GivenWalls(
        Size size, Coordinate ghostStartCoord, IEnumerable<Wall> walls, IEnumerable<Coordinate> expectedPosCoords)
    {
        var gameState = _gameState with
        {
            Size = size,
            Ghosts = new BaseGhost[] {new RandomGhost(ghostStartCoord, It.IsAny<int>(), _mockSelector.Object)},
            Walls = walls
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.SelectFrom(Capture.With(match)));
        gameState.Ghosts.Single().Move(gameState);
        
        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void Move_MovesGhostToRandomValidPosition_GivenGhosts(
        Size size, Coordinate ghostStartCoord, IEnumerable<BaseGhost> ghosts, IEnumerable<Coordinate> expectedPosCoords)
    {
        var gameState = _gameState with
        {
            Size = size,
            Ghosts = new BaseGhost[] {new RandomGhost(ghostStartCoord, It.IsAny<int>(), _mockSelector.Object)}.Concat(ghosts),
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.SelectFrom(Capture.With(match)));
        gameState.Ghosts.First().Move(gameState);
        
        Assert.Equal(expectedPosCoords, actualPosCoords);
    }

    private static IEnumerable<object[]> WallsTestData()
    {
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1),
            new Wall[] {new(new Coordinate(1, 0))},
            new Coordinate[] {new(1, 2), new(2, 1), new(0, 1)}
        };
        
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1),
            new Wall[] {new(new Coordinate(2, 1))},
            new Coordinate[] {new(1, 0), new(1, 2), new(0, 1)}
        };
        
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1),
            new Wall[] {new(new Coordinate(1, 2))},
            new Coordinate[] {new(1, 0), new(2, 1), new(0, 1)}
        };
        
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1),
            new Wall[] {new(new Coordinate(0, 1))},
            new Coordinate[] {new(1, 0), new(1, 2), new(2, 1)}
        };
    }

    private static IEnumerable<object[]> GhostsTestData()
    {
        yield return new object[]
        {
            new Size(3, 3),
            new Coordinate(1, 1),
            new BaseGhost[] {new RandomGhost(new Coordinate(1, 0), It.IsAny<int>(), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate[] {new(1, 2), new(2, 1), new(0, 1)}
        };
    }
}
