using System;
using System.Collections.Generic;
using Moq;
using Pacman.Business.Control;
using Pacman.Business.Model;
using Pacman.Business.View;
using Pacman.Variables;
using Xunit;

namespace Pacman.Tests;

public class PacTests
{
    private readonly Mock<IReader> _reader = new();
    private readonly Mock<IWriter> _writer = new();

    [Theory]
    [MemberData(nameof(PrimitiveMoveTestData))]
    public void Move_ShouldMoveInRespectiveDirection_WhenKeyIsPressed(
        Coordinate startingCoord, string keyPress, Coordinate expectedCoord)
    {
        var pac = new Pac(startingCoord, _reader.Object, _writer.Object);
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            Array.Empty<Wall>(),
            Array.Empty<MovableEntity>()
        );
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        pac.Move(gameState);
        
        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, pac.Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WrappingTestData))]
    public void Move_ShouldWrapCoordinate_WhenKeyIsPressedAndOnEdge(
        Coordinate startingCoord, Size mapSize, string keyPress, Coordinate expectedCoord)
    {
        var pac = new Pac(startingCoord, _reader.Object, _writer.Object);
        var gameState = new GameState(
            mapSize,
            pac,
            Array.Empty<Wall>(),
            Array.Empty<MovableEntity>()
        );
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        pac.Move(gameState);
        
        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, pac.Coordinate);
    }

    [Fact]
    public void Move_ShouldContinuouslyQueryForKey_WhenKeyPressIsInvalid()
    {
        const string invalidKeyPress = "";
        const string validKeyPress = Constants.UpKey;
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var gameState = new GameState(
            new Size(3, 3),
            pac,
            Array.Empty<Wall>(),
            Array.Empty<MovableEntity>()
        );
        _reader.SetupSequence(_ => _.ReadKey()).Returns(invalidKeyPress).Returns(validKeyPress);
        
        pac.Move(gameState);
        
        _writer.Verify(_ => _.WriteLine(Messages.InvalidKeyPress), Times.Once);
        _reader.Verify(_ => _.ReadKey(), Times.Exactly(2));
        Assert.Equal(new Coordinate(1, 0), pac.Coordinate);
    }
    
    [Fact]
    public void Move_ShouldContinuouslyQueryForKey_WhenMovingIntoAWall()
    {
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        var gameState = new GameState(
            new Size(3, 3), 
            pac,
            new Wall[] {new (new Coordinate(1, 0))}, 
            Array.Empty<MovableEntity>()
            );
        _reader.SetupSequence(_ => _.ReadKey()).Returns(Constants.UpKey).Returns(Constants.DownKey);
        
        pac.Move(gameState);
        
        _writer.Verify(_ => _.WriteLine(Messages.WallObstruction), Times.Once);
        _reader.Verify(_ => _.ReadKey(), Times.Exactly(2));
        Assert.Equal(new Coordinate(1, 2), pac.Coordinate);
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
