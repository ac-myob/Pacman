using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Pacman.Business.Control.Ghosts;
using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Control.Selector;
using Pacman.Business.Model;
using Xunit;
using Capture = Moq.Capture;

namespace Pacman.Tests.MovableEntityTests;

public class RandomGhostTests
{
    private readonly Mock<ISelector<Coordinate>> _mockSelector = new();
    private readonly IMoveStrategy _moveStrategy;

    public RandomGhostTests()
    {
        _moveStrategy = new RandomMoveStrategy(_mockSelector.Object);
    }
    
    [Fact]
    public void PlayTurn_MovesGhostToRandomValidPosition_GivenNoObstacles()
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac(),
            Ghosts = new[]
            {
                TestHelper.GetGhost() with
                {
                    Coordinate = new Coordinate(1, 1), 
                    MoveStrategy = _moveStrategy,
                    Id = 1
                }
            }
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
        gameState.Ghosts.Single().PlayTurn(gameState);

        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(WallsTestData))]
    public void PlayTurn_MovesGhostToRandomValidPosition_GivenWalls(
        Size size, Coordinate ghostStartCoord, IEnumerable<Wall> walls, IEnumerable<Coordinate> expectedPosCoords)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = size,
            Pac = TestHelper.GetPac(),
            Ghosts = new[]
            {
                TestHelper.GetGhost() with
                {
                    Coordinate = ghostStartCoord, 
                    MoveStrategy = _moveStrategy,
                    Id = 1
                }
            },
            Walls = walls
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.SelectFrom(Capture.With(match)));
        gameState.Ghosts.Single().PlayTurn(gameState);
        
        Assert.Equal(expectedPosCoords, actualPosCoords);
    }
    
    [Theory]
    [MemberData(nameof(GhostsTestData))]
    public void PlayTurn_MovesGhostToRandomValidPosition_GivenGhosts(
        Size size, Coordinate ghostStartCoord, IEnumerable<Ghost> ghosts, IEnumerable<Coordinate> expectedPosCoords)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = size,
            Pac = TestHelper.GetPac(),
            Ghosts = new[]
            {
                TestHelper.GetGhost() with
                {
                    Coordinate = ghostStartCoord, 
                    MoveStrategy = _moveStrategy,
                    Id = 1
                }
            }.Concat(ghosts)
        };
        IEnumerable<Coordinate> actualPosCoords = Array.Empty<Coordinate>();
        var match = new CaptureMatch<IEnumerable<Coordinate>>(f => actualPosCoords = f);
        
        _mockSelector.Setup(_ => _.SelectFrom(Capture.With(match)));
        gameState.Ghosts.First().PlayTurn(gameState);
        
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
            new[] {TestHelper.GetGhost() with{Coordinate = new Coordinate(1, 0)}},
            new Coordinate[] {new(1, 2), new(2, 1), new(0, 1)}
        };
    }
}
