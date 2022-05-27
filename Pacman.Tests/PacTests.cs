using System.Collections;
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

        pac.Move();
        
        Assert.Equal(expectedCoord, pac.Coordinate);
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
}