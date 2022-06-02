using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Xunit;
using Capture = Moq.Capture;

namespace Pacman.Tests;

public class RandomGhostTests
{
    private readonly Mock<ISelector<Coordinate>> _mockSelector = new();
    
    [Fact]
    public void Move_MovesGhostToRandomValidPosition_GivenNoObstacles()
    {
        var randomGhost = new RandomGhost(new Coordinate(1, 1), _mockSelector.Object);
        var gameState = new GameState(
            new Size(3, 3),
            It.IsAny<Pac>(),
            Array.Empty<Wall>(),
            new List<Pellet>(),
            Array.Empty<MovableEntity>()
            );
        var expectedPosCoords = new[]
        {
            new Coordinate(1, 0), 
            new Coordinate(1, 2), 
            new Coordinate(2, 1), 
            new Coordinate(0, 1)
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);

        _mockSelector.Setup(_ => _.Select(Capture.With(match)));
        randomGhost.Move(gameState);

        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void Move_MovesGhostToRandomValidPosition_GivenWalls(
        Size size, Coordinate ghostStartCoord, IEnumerable<Wall> walls, IEnumerable<Coordinate> expectedPosCoords)
    {
        var randomGhost = new RandomGhost(ghostStartCoord, _mockSelector.Object);
        var gameState = new GameState(
            size,
            It.IsAny<Pac>(),
            walls,
            new List<Pellet>(),
            Array.Empty<MovableEntity>()
        );
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.Select(Capture.With(match)));
        randomGhost.Move(gameState);
        
        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void Move_MovesGhostToRandomValidPosition_GivenGhosts(
        Size size, Coordinate ghostStartCoord, IList<MovableEntity> ghosts, IEnumerable<Coordinate> expectedPosCoords)
    {
        var randomGhost = new RandomGhost(ghostStartCoord, _mockSelector.Object);
        var gameState = new GameState(
            size,
            It.IsAny<Pac>(),
            Array.Empty<Wall>(),
            new List<Pellet>(),
            ghosts
        );
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.Select(Capture.With(match)));
        randomGhost.Move(gameState);
        
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
            new MovableEntity[] {new RandomGhost(new Coordinate(1, 0), It.IsAny<ISelector<Coordinate>>())},
            new Coordinate[] {new(1, 2), new(2, 1), new(0, 1)}
        };
    }
}
