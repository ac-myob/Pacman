using System.Collections.Generic;
using Moq;
using Pacman.Business.Control.MoveStrategies;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests.MovableEntityTests;

public class PacTests
{
    private readonly Mock<IReader> _reader = new();
    private readonly Mock<IWriter> _writer = new();
    private readonly IMoveStrategy _moveStrategy;

    public PacTests()
    {
        _moveStrategy = new PacMoveStrategy(_reader.Object, _writer.Object);
    }


    [Theory]
    [MemberData(nameof(PrimitiveMoveTestData))]
    public void PlayTurn_ShouldMoveInRespectiveDirection_WhenKeyIsPressed(
        Coordinate pacCoord, string keyPress, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac() with {Coordinate = pacCoord, MoveStrategy = _moveStrategy}
        };
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        var actualGameState = gameState.Pac.PlayTurn(gameState);

        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, actualGameState.Pac.Coordinate);
    }

    [Theory]
    [MemberData(nameof(WrappingTestData))]
    public void PlayTurn_ShouldWrapCoordinate_WhenKeyIsPressedAndOnEdge(
        Coordinate pacCoord, Size mapSize, string keyPress, Coordinate expectedCoord)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = mapSize,
            Pac = TestHelper.GetPac() with {Coordinate = pacCoord, MoveStrategy = _moveStrategy}
        };
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        var actualGameState = gameState.Pac.PlayTurn(gameState);

        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, actualGameState.Pac.Coordinate);
    }

    [Fact]
    public void PlayTurn_ShouldContinuouslyQueryForKey_WhenKeyPressIsInvalid()
    {
        const string invalidKeyPress = "";
        const string validKeyPress = Constants.UpKey;
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac() with {Coordinate = new Coordinate(1, 1), MoveStrategy = _moveStrategy}
        };
        _reader.SetupSequence(_ => _.ReadKey()).Returns(invalidKeyPress).Returns(validKeyPress);

        var actualGameState = gameState.Pac.PlayTurn(gameState);

        _writer.Verify(_ => _.Write(Messages.InvalidKeyPress), Times.Once);
        _reader.Verify(_ => _.ReadKey(), Times.Exactly(2));
        Assert.Equal(new Coordinate(1, 0), actualGameState.Pac.Coordinate);
    }

    [Fact]
    public void PlayTurn_ShouldContinuouslyQueryForKey_WhenMovingIntoAWall()
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 3),
            Pac = TestHelper.GetPac() with {Coordinate = new Coordinate(1, 1), MoveStrategy = _moveStrategy},
            Walls = new Wall[] {new(new Coordinate(1, 0))}
        };
        _reader.SetupSequence(_ => _.ReadKey()).Returns(Constants.UpKey).Returns(Constants.DownKey);

        var actualGameState = gameState.Pac.PlayTurn(gameState);

        _writer.Verify(_ => _.Write(Messages.WallObstruction), Times.Once);
        _reader.Verify(_ => _.ReadKey(), Times.Exactly(2));
        Assert.Equal(new Coordinate(1, 2), actualGameState.Pac.Coordinate);
    }

    [Theory]
    [InlineData(Constants.UpKey, Constants.PacUp)]
    [InlineData(Constants.DownKey, Constants.PacDown)]
    [InlineData(Constants.LeftKey, Constants.PacLeft)]
    [InlineData(Constants.RightKey, Constants.PacRight)]
    public void PlayTurn_ShouldUpdatePacSymbolBasedOnDirection_WhenKeyPressed(string keyPress, char expectedSymbol)
    {
        var gameState = TestHelper.GetGameState() with
        {
            Size = new Size(3, 4),
            Pac = TestHelper.GetPac() with {Coordinate = new Coordinate(1, 1), MoveStrategy = _moveStrategy}
        };
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        var actualGameState = gameState.Pac.PlayTurn(gameState);

        Assert.Equal(expectedSymbol, actualGameState.Pac.Symbol);
    }

    private static IEnumerable<object[]> PrimitiveMoveTestData()
    {
        yield return new object[]
        {
            new Coordinate(1, 1),
            Constants.UpKey,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Constants.DownKey,
            new Coordinate(1, 2)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Constants.LeftKey,
            new Coordinate(0, 1)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            Constants.RightKey,
            new Coordinate(2, 1)
        };
    }

    private static IEnumerable<object[]> WrappingTestData()
    {
        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            Constants.UpKey,
            new Coordinate(0, 1)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            Constants.DownKey,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            Constants.LeftKey,
            new Coordinate(1, 0)
        };

        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            Constants.RightKey,
            new Coordinate(0, 1)
        };
    }
}