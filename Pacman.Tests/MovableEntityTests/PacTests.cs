using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests.MovableEntityTests;

public class PacTests
{
    [Theory]
    [MemberData(nameof(PrimitiveMoveTestData))]
    public void PlayTurn_ShouldMoveInRespectiveDirection_WhenKeyIsPressed(
        Coordinate pacCoord, Direction direction, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = new Pac(pacCoord, It.IsAny<char>())
        };

        gameState.Pac.PlayTurn(gameState, direction);
        
        Assert.Equal(expectedCoord, gameState.Pac.Coordinate);
    }

    [Theory]
    [MemberData(nameof(WrappingTestData))]
    public void PlayTurn_ShouldWrapCoordinate_WhenKeyIsPressedAndOnEdge(
        Coordinate pacCoord, Size mapSize, Direction direction, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = mapSize,
            Pac = new Pac(pacCoord, It.IsAny<char>())
        };

        gameState.Pac.PlayTurn(gameState, direction);
        
        Assert.Equal(expectedCoord, gameState.Pac.Coordinate);
    }

    [Theory]
    [InlineData(Direction.North, Constants.PacUp)]
    [InlineData(Direction.South, Constants.PacDown)]
    [InlineData(Direction.East, Constants.PacRight)]
    [InlineData(Direction.West, Constants.PacLeft)]
    public void PlayTurn_ShouldUpdatePacSymbolBasedOnDirection_WhenKeyPressed(Direction direction, char expectedSymbol)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 4),
            Pac = new Pac(new Coordinate(1, 1), It.IsAny<char>())
        };

        gameState.Pac.PlayTurn(gameState, direction);

        Assert.Equal(expectedSymbol, gameState.Pac.Symbol);
    }

    private static IEnumerable<object[]> PrimitiveMoveTestData()
    {
        yield return new object[]
        {
            new Coordinate(1, 1),
            Direction.North,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Direction.South,
            new Coordinate(1, 2)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Direction.West,
            new Coordinate(0, 1)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Direction.East,
            new Coordinate(2, 1)
        };
    }

    private static IEnumerable<object[]> WrappingTestData()
    {
        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            Direction.North,
            new Coordinate(0, 1)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            Direction.South,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            Direction.West,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            Direction.East,
            new Coordinate(0, 1)
        };
    }
}