using System.Collections.Generic;
using Moq;
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
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        pac.Move(new Size(3, 3));
        
        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, pac.Coordinate);
    }
    
    [Theory]
    [MemberData(nameof(WrappingTestData))]
    public void Move_ShouldWrapCoordinate_WhenKeyIsPressedAndOnEdge(
        Coordinate startingCoord, Size mapSize, string keyPress, Coordinate expectedCoord)
    {
        var pac = new Pac(startingCoord, _reader.Object, _writer.Object);
        _reader.Setup(_ => _.ReadKey()).Returns(keyPress);

        pac.Move(mapSize);
        
        _reader.Verify(_ => _.ReadKey(), Times.Once);
        Assert.Equal(expectedCoord, pac.Coordinate);
    }

    [Fact]
    public void Move_ShouldContinuouslyQueryForKey_WhenKeyPressIsInvalid()
    {
        const string invalidKeyPress = "";
        const string validKeyPress = "UpArrow";
        var pac = new Pac(new Coordinate(1, 1), _reader.Object, _writer.Object);
        _reader.SetupSequence(_ => _.ReadKey()).Returns(invalidKeyPress).Returns(validKeyPress);
        
        pac.Move(new Size(3, 3));
        
        _writer.Verify(_ => _.WriteLine(Messages.InvalidKeyPress), Times.Once);
        _reader.Verify(_ => _.ReadKey(), Times.Exactly(2));
        Assert.Equal(new Coordinate(1, 0), pac.Coordinate);
    }

    private static IEnumerable<object[]> PrimitiveMoveTestData()
    {
        yield return new object[]
        {
            new Coordinate(1, 1),
            "UpArrow",
            new Coordinate(1, 0)
        };
        
        yield return new object[]
        {
            new Coordinate(1, 1),
            "DownArrow",
            new Coordinate(1, 2)
        };
        
        yield return new object[]
        {
            new Coordinate(1, 1),
            "LeftArrow",
            new Coordinate(0, 1)
        };
        
        yield return new object[]
        {
            new Coordinate(1, 1),
            "RightArrow",
            new Coordinate(2, 1)
        };
    }
    
    private static IEnumerable<object[]> WrappingTestData()
    {
        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            "UpArrow",
            new Coordinate(0, 1)
        };
        
        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            "DownArrow",
            new Coordinate(1, 0)
        };
        
        yield return new object[]
        {
            new Coordinate(0, 0),
            new Size(2, 2),
            "LeftArrow",
            new Coordinate(1, 0)
        };
        
        yield return new object[]
        {
            new Coordinate(1, 1),
            new Size(2, 2),
            "RightArrow",
            new Coordinate(0, 1)
        };
    }
}